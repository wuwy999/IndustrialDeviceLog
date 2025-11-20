using IndustrialDeviceLog.Commands;
using IndustrialDeviceLog.Data;
using IndustrialDeviceLog.Helpers;
using IndustrialDeviceLog.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IndustrialDeviceLog.ViewModels
{
    /// <summary>
    /// 业务处理逻辑
    /// </summary>
    public class LogViewModel : ViewModelBase
    {
        // 表单：设备编号
        private string _deviceCode;
        public string DeviceCode
        {
            get => _deviceCode;
            set => SetProperty(ref _deviceCode, value);
        }

        // 表单：设备类型（下拉列表选项）
        public ObservableCollection<string> DeviceTypeList { get; } = new() { "AGV", "机械臂", "PLC", "复合机器人" };
        private string _selectedDeviceType;
        public string SelectedDeviceType
        {
            get => _selectedDeviceType;
            set => SetProperty(ref _selectedDeviceType, value);
        }

        // 表单：运行状态（下拉列表选项）
        public ObservableCollection<string> RunStatusList { get; } = new() { "正常", "故障", "异常" };
        private string _selectedRunStatus;
        public string SelectedRunStatus
        {
            get => _selectedRunStatus;
            set => SetProperty(ref _selectedRunStatus, value);
        }

        // 表单：日志内容
        private string _logContent;
        public string LogContent
        {
            get => _logContent;
            set => SetProperty(ref _logContent, value);
        }

        // 筛选：时间范围（开始/结束时间）
        private DateTime _startDate = DateTime.Now.AddDays(-7);
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        // 日志列表
        private ObservableCollection<DeviceLog> _logList;
        public ObservableCollection<DeviceLog> LogList
        {
            get => _logList;
            set => SetProperty(ref _logList, value);
        }

        public ICommand SaveLogCommand { get; }
        public ICommand QueryLogCommand { get; }
        public ICommand ExportExcelCommand { get; }
        public ICommand DeleteLogCommand { get; }

        private readonly AppDbContext _dbContext;


        public LogViewModel()
        {
            _dbContext = new AppDbContext() ?? throw new InvalidOperationException("数据库上下文初始化失败");
            // 初始化命令（绑定执行逻辑）
            SaveLogCommand = new RelayCommand<DeviceLog>(ExecuteSaveLog) ?? throw new InvalidOperationException("命令初始化失败");
            QueryLogCommand = new RelayCommand<DeviceLog>(ExecuteQueryLog) ?? throw new InvalidOperationException("命令初始化失败");
            ExportExcelCommand = new RelayCommand<DeviceLog>(ExecuteExportExcel) ?? throw new InvalidOperationException("命令初始化失败");
            DeleteLogCommand = new RelayCommand<DeviceLog>(ExecuteDeleteLog);

            LogList = new ObservableCollection<DeviceLog>(_dbContext.DeviceLogs.ToList());

        }


        private void ExecuteSaveLog(DeviceLog logToSave)
        {

            if (string.IsNullOrWhiteSpace(DeviceCode) || string.IsNullOrWhiteSpace(SelectedDeviceType))
            {
                MessageBox.Show("设备编号和类型不能为空！");
                return;
            }

            var newLog = new DeviceLog
            {
                DeviceCode = DeviceCode,
                DeviceType = SelectedDeviceType,
                DeviceRunStatus = SelectedRunStatus,
                DeviceLogContent = LogContent,
                DeviceLogCreatTime = DateTime.Now
            };

            _dbContext.DeviceLogs.Add(newLog);
            _dbContext.SaveChanges();


            LogList.Add(newLog);

            // 清空表单
            DeviceCode = "";
            SelectedDeviceType = "";
            SelectedRunStatus = "";
            LogContent = "";

            MessageBox.Show("日志保存成功！");
        }

        // 执行“查询日志”
        private void ExecuteQueryLog(DeviceLog logToQuery)
        {
            // 从数据库筛选（按设备类型、时间范围）
            var query = _dbContext.DeviceLogs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(SelectedDeviceType))
            {
                query = query.Where(l => l.DeviceType == SelectedDeviceType);
            }

            query = query.Where(l => l.DeviceLogCreatTime >= StartDate && l.DeviceLogCreatTime <= EndDate);

            var result = query.OrderByDescending(l => l.DeviceLogCreatTime).ToList();

            // 更新界面列表
            LogList = new ObservableCollection<DeviceLog>(result);
        }

        // 执行“导出Excel”
        private void ExecuteExportExcel(DeviceLog logToExport)
        {
            if (LogList == null || LogList.Count == 0)
            {
                MessageBox.Show("没有日志数据可导出！");
                return;
            }

            // 1 将 ObservableCollection<DeviceLog> 转换为 List<DeviceLog>（因为 ToDataTable 接收 List）
            var logList = LogList.ToList();

            // 2 转换为 DataTable
            var dataTable = ExcelHelper.ToDataTable(logList);

            // 3 让用户选择保存路径
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel 文件 (*.xlsx)|*.xlsx",
                FileName = $"设备日志_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                // 调用工具类导出
                bool isSuccess = ExcelHelper.ExportToExcel(dataTable, filePath);
                if (isSuccess)
                {
                    MessageBox.Show($"导出成功！文件路径：{filePath}");
                }
            }
        }
        private void ExecuteDeleteLog(DeviceLog logToDelete)
        {
            if (logToDelete == null) return;

            _dbContext.DeviceLogs.Remove(logToDelete);
            _dbContext.SaveChanges();

            //从界面列表移除（同步更新）
            LogList.Remove(logToDelete);

            MessageBox.Show("日志删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
