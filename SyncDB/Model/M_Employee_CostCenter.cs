using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB.Model
{
    class M_Employee_CostCenter
    {
        public long ID { get; set; }
        public string EmployNo { get; set; }
        public string CostCenter_AMS { get; set; }
        public string CostCenter_IT { get; set; }
        public string CostCenter_EXPROD { get; set; }
        public Nullable<System.DateTime> UpdateDate_AMS { get; set; }
        public Nullable<System.DateTime> UpdateDate_IT { get; set; }
        public Nullable<System.DateTime> UpdateDate_EXPROD { get; set; }
        public string Update_ID { get; set; }
    }
}
