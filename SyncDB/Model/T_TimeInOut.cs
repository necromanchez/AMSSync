using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB.Model
{
    class T_TimeInOut
    {
        public long ID { get; set; }
        public string Employee_RFID { get; set; }
        public Nullable<long> ScheduleID { get; set; }
        public Nullable<System.DateTime> TimeIn { get; set; }
        public Nullable<System.DateTime> TimeOut { get; set; }
        public long LineID { get; set; }
        public long ProcessID { get; set; }
        public Nullable<System.DateTime> DTR_TimeIn { get; set; }
        public Nullable<System.DateTime> DTR_TimeOut { get; set; }
        public string DTR_RefNo { get; set; }
        public string CSRef_No { get; set; }
        public Nullable<long> CS_ScheduleID { get; set; }
    }
}
