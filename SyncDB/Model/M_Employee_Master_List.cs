﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB.Model
{
    class M_Employee_Master_List
    {
        public long ID { get; set; }
        public string REFID { get; set; }
        public string ADID { get; set; }
        public string EmpNo { get; set; }
        public string Family_Name_Suffix { get; set; }
        public string Family_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Date_Hired { get; set; }
        public string Date_Resigned { get; set; }
        public string Status { get; set; }
        public string Emp_Category { get; set; }
        public string Date_Regularized { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string RFID { get; set; }
        public string EmployeePhoto { get; set; }
        public string Section { get; set; }
        public string Department { get; set; }
        public string Company { get; set; }
        public string CostCode { get; set; }
    }
}
