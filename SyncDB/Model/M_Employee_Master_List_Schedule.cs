using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB.Model
{
    class M_Employee_Master_List_Schedule
    {
        public long ID { get; set; }
        public string EmployeeNo { get; set; }
        public Nullable<long> ScheduleID { get; set; }
        public string UpdateID { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
