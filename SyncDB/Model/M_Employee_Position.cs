using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB.Model
{
    class M_Employee_Position
    {
        public long ID { get; set; }
        public string EmployNo { get; set; }
        public string Position { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string Update_ID { get; set; }
    }
}
