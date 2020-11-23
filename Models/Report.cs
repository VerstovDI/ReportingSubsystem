using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingSubsystem.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public DateTime ReportDateAndTime { get; set; }
        public string ReportStatus { get; set; }

        //public DataSource dataSource { get; set; } // ссылка на объект, поставляющий данные для отчета
        public Report(int id, string name, DateTime creationDateAndTime, string status) {
            this.ReportId = id;
            this.ReportName = name;
            this.ReportDateAndTime = creationDateAndTime;
            this.ReportStatus = status;
        } 
    }
}
