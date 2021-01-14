using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB.Model
{
    class M_Skills
    {
        public long ID { get; set; }
        public Nullable<long> Line { get; set; }
        public string Skill { get; set; }
        public string SkillLogo { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreateID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateID { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string ManSup { get; set; }
    }
}
