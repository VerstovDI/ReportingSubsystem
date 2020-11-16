using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingSubsystem.Models
{
    public class Report
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime creationDateAndTime { get; set; }
        //public DataSource dataSource { get; set; } // ссылка на объект, поставляющий данные для отчета
    }
}
