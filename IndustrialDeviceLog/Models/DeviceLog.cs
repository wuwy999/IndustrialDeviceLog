using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndustrialDeviceLog.Models
{
    [Table("DeviceLog")]
    public class DeviceLog
    {
        public int DeviceLogId { get; private set; }
        public string? DeviceCode { get; set; }                       //--eg: AGV-001
        public string? DeviceType { get; set; }                          //--机械臂/AGV
        public string? DeviceRunStatus { get; set; }                 //--正常/故障
        public string? DeviceLogContent { get; set; }                   //--AGV定位偏差为±0.1
        public DateTime DeviceLogCreatTime { get; set; } = DateTime.Now;
    }
}


