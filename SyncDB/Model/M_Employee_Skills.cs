using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB.Model
{
    class M_Employee_Skills
    {
        public long ID { get; set; }
        public string EmpNo { get; set; }
        public long LineID { get; set; }
        public long SkillID { get; set; }
        public string CreateID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateID { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
