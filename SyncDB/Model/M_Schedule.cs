using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB.Model
{
    class M_Schedule
    {
        public long ID { get; set; }
        public string Type { get; set; }
        public string Timein { get; set; }
        public string TimeOut { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }
        public string CreateID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateID { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
