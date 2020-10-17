using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActReport.Core.Entities
{
  public class Activity : EntityObject
    {
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ActivityText { get; set; }
        [ForeignKey("Employee_Id")]
        public virtual Employee Employee { get; set; }
        public int Employee_Id { get; set; }

        public Activity()
        {
            Date = DateTime.Now;
            StartTime = DateTime.Parse("01.01.1900 00:00");
            EndTime = DateTime.Parse("01.01.1900 00:00");
        }
    }
}
