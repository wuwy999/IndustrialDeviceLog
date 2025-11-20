using IndustrialDeviceLog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IndustrialDeviceLog.Data
{
    /// <summary>
    /// 连接数据库类
    /// </summary>
    public class AppDbContext : DbContext
    {
        // 3. DbSet<T>：映射“C#实体类”与“数据表”
        public DbSet<DeviceLog> DeviceLogs { get; set; }

        // 告诉 EF Core 连接哪个数据库
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string? assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //基于程序集位置构建配置文件路径
            var configuration = new ConfigurationBuilder()
            .SetBasePath(assemblyLocation)
            .AddJsonFile("appsettings.json")
            .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}


