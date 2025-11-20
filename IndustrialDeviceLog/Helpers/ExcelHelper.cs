using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IndustrialDeviceLog.Helpers
{
    public static class ExcelHelper
    {
        /// <summary>
        /// 将 List<T> 转换为 DataTable（可复用）
        /// </summary>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            // 获取 T 的所有属性，作为 DataTable 的列
            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                var columnType = prop.PropertyType == typeof(DateTime) ? typeof(string) : prop.PropertyType;
                dataTable.Columns.Add(prop.Name, columnType);
            }
            // 填充数据行
            foreach (var item in items)
            {
                var row = dataTable.NewRow();
                foreach (var prop in properties)
                {

                    var value = prop.GetValue(item);

                    if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {

                        if (value != null)
                        {
                            row[prop.Name] = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {

                            row[prop.Name] = DBNull.Value;

                        }
                    }
                    else
                    {
                        row[prop.Name] = value ?? DBNull.Value;
                    }
                }
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

        /// <summary>
        /// 将 DataTable 导出为 Excel 文件 -  数据从DataTable向其他表格结构（如 Excel 表格）迁移
        /// </summary>
        public static bool ExportToExcel(DataTable dataTable, string filePath)
        {
            try
            {
                // 创建 XLSX 格式工作簿（支持 2007+ 版本）
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("设备日志");

                //1 写入表头（第一行）
                IRow headerRow = sheet.CreateRow(0);
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(dataTable.Columns[i].ColumnName);
                }

                //2 写入数据行（从第二行开始）
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        dataRow.CreateCell(j).SetCellValue(dataTable.Rows[i][j]?.ToString() ?? "");

                    }
                }

                // 3 自动调整列宽（适配内容）
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                // 4 保存到文件  
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    workbook.Write(fileStream);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败：{ex.Message}");
                return false;
            }
        }
    }
}
