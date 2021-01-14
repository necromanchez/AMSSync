using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB.Model
{
    class M_LineTeam
    {
        public long ID { get; set; }
        public string Section { get; set; }
        public string Line { get; set; }
        public Nullable<bool> Status { get; set; }
        public bool IsDeleted { get; set; }
        public string CreateID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateID { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
