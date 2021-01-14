using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;

namespace SyncDB.Config
{
    public class AppConfig : Application
    {
        public static string databaseName = "SQLlite_AMSBrother.sqlite";
        public static string forderPath = AppDomain.CurrentDomain.BaseDirectory; //
        public static string databasepath = "Data Source=SQLlite_AMSBrother.sqlite;Version=3;";//"Data Source=" + Path.Combine(forderPath, databaseName) + ";Version=3;";

        public static string AMSDB
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Brothers_AMSDB"].ConnectionString;
            }
        }
    }

    
}
