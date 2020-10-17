using System.Collections.Generic;

namespace ActReport.Core.Entities
{
  public class Employee : EntityObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Activity> Activities { get; set; } 
    }
}
