using SyncDB.Model;
//using SQLite;
using SyncDB.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace SyncDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            myGif.Source = new Uri(AppConfig.forderPath + "\\Resources\\migration.gif");
            try
            {
               
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                Assembly curAssembly = Assembly.GetExecutingAssembly();
                key.SetValue(curAssembly.GetName().Name, curAssembly.Location);

                CloseWindow();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                MessageBox.Show("AMS Data Failed to Sync");
                this.Close();
                System.Windows.Application.Current.Shutdown();
            }
        }



        private async void CloseWindow()
        {
            await ClosingTasks();
        }

        private async Task ClosingTasks()
        {
            await Task.Delay(3000);
            SyncAll();
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void myGif_MediaEnded(object sender, RoutedEventArgs e)
        {
            //myGif.Source = new Uri(AppConfig.forderPath + "\\Resources\\migration.gif");
            myGif.Position = new TimeSpan(0, 0, 1);
            myGif.Play();
        }

        public void SyncAll()
        {
            createSQLiteDB();
            Sync_M_Employee_Master_List();
            Sync_M_Employee_Master_List_Schedule();
            Sync_M_Employee_Skills();
            Sync_M_LineTeam();
            Sync_M_Schedule();
            Sync_M_Skills();
            Sync_M_Employee_Status();
            Sync_M_Employee_Position();
            Sync_M_Employee_CostCenter();
            Sync_T_TimeInOut();
            Sync_ResourcePicture();
        }
        
        public void createSQLiteDB()
        {
            String db = AppConfig.databaseName;
            if (!File.Exists(db))
            {
                try
                {
                    SQLiteConnection.CreateFile(db);
                }
                catch (SQLiteException err)
                {
                    MessageBox.Show(err.Message);
                }
            }
            else
            {
                try
                {
                    SQLiteConnection conn = new SQLiteConnection(AppConfig.databasepath);
                    SQLiteCommand cmdSql = new SQLiteCommand();
                    cmdSql.Connection = conn;
                    cmdSql.CommandText =    "DROP TABLE M_Employee_Master_List;" +
                                            "DROP TABLE M_Employee_Master_List_Schedule;" +
                                            "DROP TABLE M_Employee_Skills;" +
                                            "DROP TABLE M_LineTeam;" +
                                            "DROP TABLE M_Schedule;" +
                                            "DROP TABLE M_Employee_Position;" +
                                            "DROP TABLE M_Employee_Status;" +
                                            "DROP TABLE M_Employee_CostCenter;" +
                                            "DROP TABLE M_Skills; ";
                    conn.Open();
                    cmdSql.ExecuteNonQuery();
                    conn.Close();
                }
                catch(Exception err)
                {
                    //File.Delete(db);
                    //createSQLiteDB();
                }
            }
        }

        public void Sync_ResourcePicture()
        {
            Process p = new Process();
            p.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\Model\\PictureResource.bat";
            //MessageBox.Show(p.StartInfo.FileName);
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
        }

        public void Sync_M_Employee_Master_List()
        {
            SqlConnection conn = new SqlConnection(AppConfig.AMSDB);
            SqlCommand cmdSql = new SqlCommand();
            cmdSql.Connection = conn;
            cmdSql.CommandText = "SELECT * FROM M_Employee_Master_List";
            conn.Open();

            SqlDataReader sdr = cmdSql.ExecuteReader();
            var dt = new DataTable();
            dt.Load(sdr);
            cmdSql.Dispose();
            conn.Close();

            List<M_Employee_Master_List> list = new List<M_Employee_Master_List>();
            list = (from rw in dt.AsEnumerable()
                    select new M_Employee_Master_List()
                    {
                        ID = Convert.ToInt64(rw["ID"]),
                        REFID = Convert.ToString(rw["REFID"]),
                        ADID = Convert.ToString(rw["ADID"]),
                        EmpNo = Convert.ToString(rw["EmpNo"]),
                        Family_Name_Suffix = Convert.ToString(rw["Family_Name_Suffix"]),
                        Family_Name = Convert.ToString(rw["Family_Name"]),
                        First_Name = Convert.ToString(rw["First_Name"]),
                        Middle_Name = Convert.ToString(rw["Middle_Name"]),
                        Date_Hired = Convert.ToString(rw["Date_Hired"]),
                        Date_Resigned = Convert.ToString(rw["Date_Resigned"]),
                        Status = Convert.ToString(rw["Status"]),
                        Emp_Category = Convert.ToString(rw["Emp_Category"]),
                        Date_Regularized = Convert.ToString(rw["Date_Regularized"]),
                        Position = Convert.ToString(rw["Position"]),
                        Email = Convert.ToString(rw["Email"]),
                        Gender = Convert.ToString(rw["Gender"]),
                        RFID = Convert.ToString(rw["RFID"]),
                        EmployeePhoto = Convert.ToString(rw["EmployeePhoto"]),
                        Section = Convert.ToString(rw["Section"]),
                        Department = Convert.ToString(rw["Department"]),
                        Company = Convert.ToString(rw["Company"]),
                        CostCode = Convert.ToString(rw["CostCode"])
                    }).ToList();

            //MessageBox.Show(AppConfig.databasepath);
            using (SQLiteConnection con = new SQLiteConnection(AppConfig.databasepath))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                #region create table
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                string sql = " CREATE TABLE M_Employee_Master_List(" +
                            "	[ID] [bigint] IDENTITY(1,1) NOT NULL," +
                            "	[REFID] [nvarchar](20) NULL," +
                            "	[ADID] [nvarchar](20) NULL," +
                            "	[EmpNo] [nvarchar](20) NULL," +
                            "	[Family_Name_Suffix] [nvarchar](50) NULL," +
                            "	[Family_Name] [nvarchar](30) NULL," +
                            "	[First_Name] [nvarchar](30) NULL," +
                            "	[Middle_Name] [nvarchar](30) NULL," +
                            "	[Date_Hired] [varchar](20) NULL," +
                            "	[Date_Resigned] [varchar](20) NULL," +
                            "	[Status] [nvarchar](50) NULL," +
                            "	[Emp_Category] [nvarchar](50) NULL," +
                            "	[Date_Regularized] [nvarchar](50) NULL," +
                            "	[Position] [nvarchar](20) NULL," +
                            "	[Email] [nvarchar](50) NULL," +
                            "	[Gender] [nvarchar](50) NULL," +
                            "	[RFID] [varchar](20) NULL," +
                            "	[EmployeePhoto] [nvarchar](100) NULL," +
                            "	[Section] [nvarchar](50) NOT NULL," +
                            "	[Department] [nvarchar](50) NULL," +
                            "	[Company] [nvarchar](50) NULL," +
                            "	[CostCode] [nvarchar](10) NULL)";

                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();
          
                #endregion
                string ch = "";
                try
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    string ins = "";
                    using (var transaction = con.BeginTransaction())
                    {
                        foreach (M_Employee_Master_List i in list)
                        {
                            ins = "INSERT INTO [M_Employee_Master_List]" +
                                          "           ([ID]" +
                                          "           ,[REFID]" +
                                          "           ,[ADID]" +
                                          "           ,[EmpNo]" +
                                          "           ,[Family_Name_Suffix]" +
                                          "           ,[Family_Name]" +
                                          "           ,[First_Name]" +
                                          "           ,[Middle_Name]" +
                                          "           ,[Date_Hired]" +
                                          "           ,[Date_Resigned]" +
                                          "           ,[Status]" +
                                          "           ,[Emp_Category]" +
                                          "           ,[Date_Regularized]" +
                                          "           ,[Position]" +
                                          "           ,[Email]" +
                                          "           ,[Gender]" +
                                          "           ,[RFID]" +
                                          "           ,[EmployeePhoto]" +
                                          "           ,[Section]" +
                                          "           ,[Department]" +
                                          "           ,[Company]" +
                                          "           ,[CostCode])" +
                                          "     VALUES" +
                                          "           (" + i.ID + "" +
                                          "           ,'" + i.REFID + "'" +
                                          "           ,'" + i.ADID + "'" +
                                          "           ,'" + i.EmpNo + "'" +
                                          "           ,'" + i.Family_Name_Suffix + "'" +
                                          "           ,'" + i.Family_Name + "'" +
                                          "           ,'" + i.First_Name + "'" +
                                          "           ,'" + i.Middle_Name + "'" +
                                          "           ,'" + i.Date_Hired + "'" +
                                          "           ,'" + i.Date_Resigned + "'" +
                                          "           ,'" + i.Status + "'" +
                                          "           ,'" + i.Emp_Category + "'" +
                                          "           ,'" + i.Date_Regularized + "'" +
                                          "           ,'" + i.Position + "'" +
                                          "           ,'" + i.Email + "'" +
                                          "           ,'" + i.Gender + "'" +
                                          "           ,'" + i.RFID + "'" +
                                          "           ,'" + i.EmployeePhoto + "'" +
                                          "           ,'" + i.Section + "'" +
                                          "           ,'" + i.Department + "'" +
                                          "           ,'" + i.Company + "'" +
                                          "           ,'" + i.CostCode + "'); ";

                            try
                            {
                                cmd.CommandText = ins;
                                ch = ins;
                                cmd.ExecuteNonQuery();
                            }
                            catch(Exception err) { }
                        }
                        
                        transaction.Commit();
                    }
                }
                catch (Exception err) {
                    string a = ch;
                }
            }

        }

        public void Sync_M_Employee_Master_List_Schedule()
        {
            SqlConnection conn = new SqlConnection(AppConfig.AMSDB);
            SqlCommand cmdSql = new SqlCommand();
            cmdSql.Connection = conn;
            cmdSql.CommandText = "SELECT * FROM M_Employee_Master_List_Schedule WHERE ScheduleID IS NOT NULL";
            conn.Open();

            SqlDataReader sdr = cmdSql.ExecuteReader();
            var dt = new DataTable();
            dt.Load(sdr);
            cmdSql.Dispose();
            conn.Close();

            List<M_Employee_Master_List_Schedule> list = new List<M_Employee_Master_List_Schedule>();
            list = (from rw in dt.AsEnumerable()
                    select new M_Employee_Master_List_Schedule()
                    {
                        ID = Convert.ToInt64(rw["ID"]),
                        EmployeeNo = Convert.ToString(rw["EmployeeNo"]),
                        ScheduleID = Convert.ToInt64(rw["ScheduleID"]),
                        UpdateID = Convert.ToString(rw["UpdateID"]),
                        UpdateDate = Convert.ToDateTime(rw["UpdateDate"])
                    }).ToList();

            //MessageBox.Show(AppConfig.databasepath);
            using (SQLiteConnection con = new SQLiteConnection(AppConfig.databasepath))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                #region create table
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                string sql ="CREATE TABLE [M_Employee_Master_List_Schedule](" +
                            "	[ID] [bigint] IDENTITY(1,1) NOT NULL," +
                            "	[EmployeeNo] [nvarchar](50) NULL," +
                            "	[ScheduleID] [bigint] NULL," +
                            "	[UpdateID] [nvarchar](50) NULL," +
                            "	[UpdateDate] [datetime] NULL" +
                            ");";

                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();

                #endregion
                string ch = "";
                try
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    string ins = "";
                    using (var transaction = con.BeginTransaction())
                    {
                        foreach (M_Employee_Master_List_Schedule i in list)
                        {
                            ins =   "INSERT INTO [M_Employee_Master_List_Schedule]" +
                                    "           ([ID]" +
                                    "           ,[EmployeeNo]" +
                                    "           ,[ScheduleID]" +
                                    "           ,[UpdateID]" +
                                    "           ,[UpdateDate])" +
                                    "VALUES" +
                                    "           ('" + i.ID + "'" +
                                    "           ,'" + i.EmployeeNo + "'" +
                                    "           ," + i.ScheduleID + "" +
                                    "           ,'" + i.UpdateID + "'" +
                                    "           ,'" + i.UpdateDate + "'); ";
                            try
                            {
                                cmd.CommandText = ins;
                                ch = ins;
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception err) { }
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception err)
                {
                    string a = ch;
                }
            }

        }

        public void Sync_M_Employee_Skills()
        {
            SqlConnection conn = new SqlConnection(AppConfig.AMSDB);
            SqlCommand cmdSql = new SqlCommand();
            cmdSql.Connection = conn;
            cmdSql.CommandText = "SELECT * FROM M_Employee_Skills";
            conn.Open();

            SqlDataReader sdr = cmdSql.ExecuteReader();
            var dt = new DataTable();
            dt.Load(sdr);
            cmdSql.Dispose();
            conn.Close();

            List<M_Employee_Skills> list = new List<M_Employee_Skills>();
            list = (from rw in dt.AsEnumerable()
                    select new M_Employee_Skills()
                    {
                        ID = Convert.ToInt64(rw["ID"]),
                        EmpNo = Convert.ToString(rw["EmpNo"]),
                        LineID = Convert.ToInt64(rw["LineID"]),
                        SkillID = Convert.ToInt64(rw["SkillID"]),
                        CreateID = Convert.ToString(rw["CreateID"]),
                        CreateDate = Convert.ToDateTime(rw["CreateDate"]),
                        UpdateID = Convert.ToString(rw["UpdateID"]),
                        UpdateDate = Convert.ToDateTime(rw["UpdateDate"])
                    }).ToList();
            
            using (SQLiteConnection con = new SQLiteConnection(AppConfig.databasepath))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                #region create table
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                string sql ="CREATE TABLE [M_Employee_Skills](" +
                            "	[ID] [bigint] IDENTITY(1,1) NOT NULL," +
                            "	[EmpNo] [nvarchar](50) NOT NULL," +
                            "	[LineID] [bigint] NOT NULL," +
                            "	[SkillID] [bigint] NOT NULL," +
                            "	[CreateID] [nvarchar](50) NOT NULL," +
                            "	[CreateDate] [datetime] NOT NULL," +
                            "	[UpdateID] [nvarchar](50) NOT NULL," +
                            "	[UpdateDate] [datetime] NOT NULL" +
                            "); ";

                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();

                #endregion
                string ch = "";
                try
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    string ins = "";
                    using (var transaction = con.BeginTransaction())
                    {
                        foreach (M_Employee_Skills i in list)
                        {

                          ins = "INSERT INTO [M_Employee_Skills]" +
                                "           ([ID]" +
                                "           ,[EmpNo]" +
                                "           ,[LineID]" +
                                "           ,[SkillID]" +
                                "           ,[CreateID]" +
                                "           ,[CreateDate]" +
                                "           ,[UpdateID]" +
                                "           ,[UpdateDate])" +
                                "VALUES" +
                                "           ('" + i.ID + "'" +
                                "           ,'" + i.EmpNo + "'" +
                                "           ," + i.LineID + "" +
                                "           ," + i.SkillID + "" +
                                "           ,'" + i.CreateID + "'" +
                                "           ,'" + i.CreateDate + "'" +
                                "           ,'" + i.UpdateID + "'" +
                                "           ,'" + i.UpdateDate + "'); ";
                            
                            try
                            {
                                cmd.CommandText = ins;
                                ch = ins;
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception err) { }
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception err)
                {
                    string a = ch;
                }
            }

        }

        public void Sync_M_LineTeam()
        {
            SqlConnection conn = new SqlConnection(AppConfig.AMSDB);
            SqlCommand cmdSql = new SqlCommand();
            cmdSql.Connection = conn;
            cmdSql.CommandText = "SELECT * FROM M_LineTeam";
            conn.Open();

            SqlDataReader sdr = cmdSql.ExecuteReader();
            var dt = new DataTable();
            dt.Load(sdr);
            cmdSql.Dispose();
            conn.Close();

            List<M_LineTeam> list = new List<M_LineTeam>();
            list = (from rw in dt.AsEnumerable()
                    select new M_LineTeam()
                    {
                        ID = Convert.ToInt64(rw["ID"]),
                        Section = Convert.ToString(rw["Section"]),
                        Line = Convert.ToString(rw["Line"]),
                        Status = Convert.ToBoolean(rw["Status"]),
                        IsDeleted = Convert.ToBoolean(rw["IsDeleted"]),
                        CreateID = Convert.ToString(rw["CreateID"]),
                        CreateDate = Convert.ToDateTime(rw["CreateDate"]),
                        UpdateID = Convert.ToString(rw["UpdateID"]),
                        UpdateDate = Convert.ToDateTime(rw["UpdateDate"])
                    }).ToList();

            using (SQLiteConnection con = new SQLiteConnection(AppConfig.databasepath))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                #region create table
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                string sql ="CREATE TABLE [M_LineTeam](" +
                            "	[ID] [bigint] IDENTITY(1,1) NOT NULL," +
                            "	[Section] [nvarchar](50) NULL," +
                            "	[Line] [nvarchar](50) NULL," +
                            "	[Status] [bit] NULL," +
                            "	[IsDeleted] [bit] NOT NULL," +
                            "	[CreateID] [nvarchar](20) NOT NULL," +
                            "	[CreateDate] [datetime] NOT NULL," +
                            "	[UpdateID] [nvarchar](20) NOT NULL," +
                            "	[UpdateDate] [datetime] NOT NULL" +
                            ");";


                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();

                #endregion
                string ch = "";
                try
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    string ins = "";
                    using (var transaction = con.BeginTransaction())
                    {
                        foreach (M_LineTeam i in list)
                        {

                          ins = "INSERT INTO [M_LineTeam]" +
                                "           ([ID]" +
                                "           ,[Section]" +
                                "           ,[Line]" +
                                "           ,[Status]" +
                                "           ,[IsDeleted]" +
                                "           ,[CreateID]" +
                                "           ,[CreateDate]" +
                                "           ,[UpdateID]" +
                                "           ,[UpdateDate])" +
                                "VALUES" +
                                "           ('" + i.ID + "'" +
                                "           ,'" + i.Section + "'" +
                                "           ,'" + i.Line + "'" +
                                "           ,'" + i.Status + "'" +
                                "           ,'" + i.IsDeleted + "'" +
                                "           ,'" + i.CreateID + "'" +
                                "           ,'" + i.CreateDate + "'" +
                                "           ,'" + i.UpdateID + "'" +
                                "           ,'" + i.UpdateDate + "');";
                            
                            try
                            {
                                cmd.CommandText = ins;
                                ch = ins;
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception err) { }
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception err)
                {
                    string a = ch;
                }
            }

        }

        public void Sync_M_Schedule()
        {
            SqlConnection conn = new SqlConnection(AppConfig.AMSDB);
            SqlCommand cmdSql = new SqlCommand();
            cmdSql.Connection = conn;
            cmdSql.CommandText = "SELECT * FROM M_Schedule";
            conn.Open();

            SqlDataReader sdr = cmdSql.ExecuteReader();
            var dt = new DataTable();
            dt.Load(sdr);
            cmdSql.Dispose();
            conn.Close();

            List<M_Schedule> list = new List<M_Schedule>();
            list = (from rw in dt.AsEnumerable()
                    select new M_Schedule()
                    {
                        ID = Convert.ToInt64(rw["ID"]),
                        Type = Convert.ToString(rw["Type"]),
                        Timein = Convert.ToString(rw["Timein"]),
                        TimeOut = Convert.ToString(rw["TimeOut"]),
                        Status = Convert.ToBoolean(rw["Status"]),
                        IsDeleted = Convert.ToBoolean(rw["IsDeleted"]),
                        CreateID = Convert.ToString(rw["CreateID"]),
                        CreateDate = Convert.ToDateTime(rw["CreateDate"]),
                        UpdateID = Convert.ToString(rw["UpdateID"]),
                        UpdateDate = Convert.ToDateTime(rw["UpdateDate"])
                    }).ToList();

            using (SQLiteConnection con = new SQLiteConnection(AppConfig.databasepath))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                #region create table
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                string sql ="CREATE TABLE [M_Schedule](" +
                            "	[ID] [bigint] IDENTITY(1,1) NOT NULL," +
                            "	[Type] [nvarchar](50) NULL," +
                            "	[Timein] [nvarchar](10) NOT NULL," +
                            "	[TimeOut] [nvarchar](10) NOT NULL," +
                            "	[Status] [bit] NOT NULL," +
                            "	[IsDeleted] [bit] NOT NULL," +
                            "	[CreateID] [nvarchar](20) NOT NULL," +
                            "	[CreateDate] [datetime] NOT NULL," +
                            "	[UpdateID] [nvarchar](20) NOT NULL," +
                            "	[UpdateDate] [datetime] NOT NULL" +
                            ");";
                
                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();

                #endregion
                string ch = "";
                try
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    string ins = "";
                    using (var transaction = con.BeginTransaction())
                    {
                        foreach (M_Schedule i in list)
                        {

                            ins =   "INSERT INTO [M_Schedule]" +
                                    "           ([ID]" +
                                    "           ,[Type]" +
                                    "           ,[Timein]" +
                                    "           ,[TimeOut]" +
                                    "           ,[Status]" +
                                    "           ,[IsDeleted]" +
                                    "           ,[CreateID]" +
                                    "           ,[CreateDate]" +
                                    "           ,[UpdateID]" +
                                    "           ,[UpdateDate])" +
                                    "VALUES" +
                                    "           ('" + i.ID + "'" +
                                    "           ,'" + i.Type + "'" +
                                    "           ,'" + i.Timein + "'" +
                                    "           ,'" + i.TimeOut + "'" +
                                    "           ,'" + i.Status + "'" +
                                    "           ,'" + i.IsDeleted + "'" +
                                    "           ,'" + i.CreateID + "'" +
                                    "           ,'" + i.CreateDate + "'" +
                                    "           ,'" + i.UpdateID + "'" +
                                    "           ,'" + i.UpdateDate + "');";

                            try
                            {
                                cmd.CommandText = ins;
                                ch = ins;
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception err) { }
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception err)
                {
                    string a = ch;
                }
            }

        }

        public void Sync_M_Skills()
        {
            SqlConnection conn = new SqlConnection(AppConfig.AMSDB);
            SqlCommand cmdSql = new SqlCommand();
            cmdSql.Connection = conn;
            cmdSql.CommandText = "SELECT * FROM M_Skills WHERE Count IS NOT NULL";
            conn.Open();

            SqlDataReader sdr = cmdSql.ExecuteReader();
            var dt = new DataTable();
            dt.Load(sdr);
            cmdSql.Dispose();
            conn.Close();

            List<M_Skills> list = new List<M_Skills>();
            list = (from rw in dt.AsEnumerable()
                    select new M_Skills()
                    {
                        ID = Convert.ToInt64(rw["ID"]),
                        Line = Convert.ToInt64(rw["Line"]),
                        Skill = Convert.ToString(rw["Skill"]),
                        SkillLogo = Convert.ToString(rw["SkillLogo"]),
                        Count = (rw["Count"] == null)?0:Convert.ToInt32(rw["Count"]),
                        IsDeleted = Convert.ToBoolean(rw["IsDeleted"]),
                        CreateID = Convert.ToString(rw["CreateID"]),
                        CreateDate = Convert.ToDateTime(rw["CreateDate"]),
                        UpdateID = Convert.ToString(rw["UpdateID"]),
                        UpdateDate = Convert.ToDateTime(rw["UpdateDate"]),
                        ManSup = Convert.ToString(rw["ManSup"])
                    }).ToList();

            using (SQLiteConnection con = new SQLiteConnection(AppConfig.databasepath))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                #region create table
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                string sql ="CREATE TABLE [M_Skills](" +
                            "	[ID] [bigint] IDENTITY(1,1) NOT NULL," +
                            "	[Line] [bigint] NULL," +
                            "	[Skill] [nvarchar](50) NULL," +
                            "	[SkillLogo] [nvarchar](100) NULL," +
                            "	[Count] [int] NULL," +
                            "	[IsDeleted] [bit] NULL," +
                            "	[CreateID] [nvarchar](20) NOT NULL," +
                            "	[CreateDate] [datetime] NOT NULL," +
                            "	[UpdateID] [nvarchar](20) NOT NULL," +
                            "	[UpdateDate] [datetime] NOT NULL," +
                            "	[ManSup] [nvarchar](10) NULL" +
                            ");";

                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();

                #endregion
                string ch = "";
                try
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    string ins = "";
                    using (var transaction = con.BeginTransaction())
                    {
                        foreach (M_Skills i in list)
                        {

                            ins =   "INSERT INTO [M_Skills]" +
                                    "           ([ID]" +
                                    "           ,[Line]" +
                                    "           ,[Skill]" +
                                    "           ,[SkillLogo]" +
                                    "           ,[Count]" +
                                    "           ,[IsDeleted]" +
                                    "           ,[CreateID]" +
                                    "           ,[CreateDate]" +
                                    "           ,[UpdateID]" +
                                    "           ,[UpdateDate]" +
                                    "           ,[ManSup])" +
                                    "VALUES" +
                                    "           ('" + i.ID + "'" +
                                    "           ,'" + i.Line + "'" +
                                    "           ,'" + i.Skill + "'" +
                                    "           ,'" + i.SkillLogo + "'" +
                                    "           ,'" + i.Count + "'" +
                                    "           ,'" + i.IsDeleted + "'" +
                                    "           ,'" + i.CreateID + "'" +
                                    "           ,'" + i.CreateDate + "'" +
                                    "           ,'" + i.UpdateID + "'" +
                                    "           ,'" + i.UpdateDate + "'" +
                                    "           ,'" + i.ManSup + "');";

                            try
                            {
                                cmd.CommandText = ins;
                                ch = ins;
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception err) { }
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception err)
                {
                    string a = ch;
                }
            }

        }

        public void Sync_M_Employee_Status()
        {
            SqlConnection conn = new SqlConnection(AppConfig.AMSDB);
            SqlCommand cmdSql = new SqlCommand();
            cmdSql.Connection = conn;
            cmdSql.CommandText = "SELECT * FROM M_Employee_Status";
            conn.Open();

            SqlDataReader sdr = cmdSql.ExecuteReader();
            var dt = new DataTable();
            dt.Load(sdr);
            cmdSql.Dispose();
            conn.Close();

            List<M_Employee_Status> list = new List<M_Employee_Status>();
            list = (from rw in dt.AsEnumerable()
                    select new M_Employee_Status()
                    {
                        ID = Convert.ToInt64(rw["ID"]),
                        EmployNo = Convert.ToString(rw["EmployNo"]),
                        Status = Convert.ToString(rw["Status"]),
                        Update_ID = Convert.ToString(rw["Update_ID"]),
                        UpdateDate = Convert.ToDateTime(rw["UpdateDate"]),
                        
                    }).ToList();

            using (SQLiteConnection con = new SQLiteConnection(AppConfig.databasepath))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                #region create table
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                string sql ="CREATE TABLE [M_Employee_Status](" +
                            "	[ID] [bigint] IDENTITY(1,1) NOT NULL," +
                            "	[EmployNo] [nvarchar](50) NULL," +
                            "	[Status] [nvarchar](50) NULL," +
                            "	[UpdateDate] [datetime] NULL," +
                            "	[Update_ID] [nvarchar](50) NULL" +
                            ");";
                
                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();

                #endregion
                string ch = "";
                try
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    string ins = "";
                    using (var transaction = con.BeginTransaction())
                    {
                        foreach (M_Employee_Status i in list)
                        {

                            ins =   "INSERT INTO [M_Employee_Status]" +
                                    "           ([ID]" +
                                    "           ,[EmployNo]" +
                                    "           ,[Status]" +
                                    "           ,[UpdateDate]" +
                                    "           ,[Update_ID])" +
                                    "VALUES" +
                                    "           ('" + i.ID + "'" +
                                    "           ,'" + i.EmployNo + "'" +
                                    "           ,'" + i.Status + "'" +
                                    "           ,'" + i.UpdateDate + "'" +
                                    "           ,'" + i.Update_ID + "');";

                            try
                            {
                                cmd.CommandText = ins;
                                ch = ins;
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception err) { }
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception err)
                {
                    string a = ch;
                }
            }

        }

        public void Sync_M_Employee_Position()
        {
            SqlConnection conn = new SqlConnection(AppConfig.AMSDB);
            SqlCommand cmdSql = new SqlCommand();
            cmdSql.Connection = conn;
            cmdSql.CommandText = "SELECT * FROM M_Employee_Position";
            conn.Open();

            SqlDataReader sdr = cmdSql.ExecuteReader();
            var dt = new DataTable();
            dt.Load(sdr);
            cmdSql.Dispose();
            conn.Close();

            List<M_Employee_Position> list = new List<M_Employee_Position>();
            list = (from rw in dt.AsEnumerable()
                    select new M_Employee_Position()
                    {
                        ID = Convert.ToInt64(rw["ID"]),
                        EmployNo = Convert.ToString(rw["EmployNo"]),
                        Position = Convert.ToString(rw["Position"]),
                        Update_ID = Convert.ToString(rw["Update_ID"]),
                        UpdateDate = Convert.ToDateTime(rw["UpdateDate"])
                    }).ToList();

            using (SQLiteConnection con = new SQLiteConnection(AppConfig.databasepath))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                #region create table
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                string sql ="CREATE TABLE [M_Employee_Position](" +
                            "	[ID] [bigint] IDENTITY(1,1) NOT NULL," +
                            "	[EmployNo] [nvarchar](50) NULL," +
                            "	[Position] [nvarchar](50) NULL," +
                            "	[UpdateDate] [datetime] NULL," +
                            "	[Update_ID] [nvarchar](50) NULL" +
                            ");";


                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();

                #endregion
                string ch = "";
                try
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    string ins = "";
                    using (var transaction = con.BeginTransaction())
                    {
                        foreach (M_Employee_Position i in list)
                        {

                            ins = "INSERT INTO [M_Employee_Position]" +
                                    "           ([ID]" +
                                    "           ,[EmployNo]" +
                                    "           ,[Position]" +
                                    "           ,[UpdateDate]" +
                                    "           ,[Update_ID])" +
                                    "VALUES" +
                                    "           ('" + i.ID + "'" +
                                    "           ,'" + i.EmployNo + "'" +
                                    "           ,'" + i.Position + "'" +
                                    "           ,'" + i.UpdateDate + "'" +
                                    "           ,'" + i.Update_ID + "');";

                            try
                            {
                                cmd.CommandText = ins;
                                ch = ins;
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception err) { }
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception err)
                {
                    string a = ch;
                }
            }

        }

        public void Sync_M_Employee_CostCenter()
        {
            SqlConnection conn = new SqlConnection(AppConfig.AMSDB);
            SqlCommand cmdSql = new SqlCommand();
            cmdSql.Connection = conn;
            cmdSql.CommandText = "SELECT * FROM M_Employee_CostCenter WHERE CostCenter_AMS<> '' AND CostCenter_AMS IS NOT NULL";
            conn.Open();

            SqlDataReader sdr = cmdSql.ExecuteReader();
            var dt = new DataTable();
            dt.Load(sdr);
            cmdSql.Dispose();
            conn.Close();

            List<M_Employee_CostCenter> list = new List<M_Employee_CostCenter>();
            list = (from rw in dt.AsEnumerable()
                    select new M_Employee_CostCenter()
                    {
                        ID = Convert.ToInt64(rw["ID"]),
                        EmployNo = Convert.ToString(rw["EmployNo"]),
                        CostCenter_AMS = Convert.ToString(rw["CostCenter_AMS"]),
                        CostCenter_IT = Convert.ToString(rw["CostCenter_IT"]),
                        CostCenter_EXPROD = Convert.ToString(rw["CostCenter_EXPROD"]),
                        Update_ID = Convert.ToString(rw["Update_ID"]),
                        UpdateDate_AMS = Convert.ToDateTime(rw["UpdateDate_AMS"]),
                        UpdateDate_IT = Convert.ToDateTime(rw["UpdateDate_IT"]),
                        UpdateDate_EXPROD = Convert.ToDateTime("1990-01-01")

                    }).ToList();

            using (SQLiteConnection con = new SQLiteConnection(AppConfig.databasepath))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                #region create table
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                string sql ="CREATE TABLE [M_Employee_CostCenter](" +
                            "	[ID] [bigint] IDENTITY(1,1) NOT NULL," +
                            "	[EmployNo] [nvarchar](50) NULL," +
                            "	[CostCenter_AMS] [nvarchar](10) NULL," +
                            "	[CostCenter_IT] [nvarchar](10) NULL," +
                            "	[CostCenter_EXPROD] [nvarchar](10) NULL," +
                            "	[UpdateDate_AMS] [datetime] NULL," +
                            "	[UpdateDate_IT] [datetime] NULL," +
                            "	[UpdateDate_EXPROD] [datetime] NULL," +
                            "	[Update_ID] [nvarchar](50) NULL" +
                            ");";


                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();

                #endregion
                string ch = "";
                try
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    string ins = "";
                    using (var transaction = con.BeginTransaction())
                    {
                        foreach (M_Employee_CostCenter i in list)
                        {

                            ins =   "INSERT INTO [M_Employee_CostCenter]" +
                                    "           ([ID]" +
                                    "           ,[EmployNo]" +
                                    "           ,[CostCenter_AMS]" +
                                    "           ,[CostCenter_IT]" +
                                    "           ,[CostCenter_EXPROD]" +
                                    "           ,[UpdateDate_AMS]" +
                                    "           ,[UpdateDate_IT]" +
                                    "           ,[UpdateDate_EXPROD]" +
                                    "           ,[Update_ID])" +
                                    "VALUES" +
                                    "           ('" + i.ID + "'" +
                                    "           ,'" + i.EmployNo + "'" +
                                    "           ,'" + i.CostCenter_AMS + "'" +
                                    "           ,'" + i.CostCenter_IT + "'" +
                                    "           ,'" + i.CostCenter_EXPROD + "'" +
                                    "           ,'" + i.UpdateDate_AMS + "'" +
                                    "           ,'" + i.UpdateDate_IT + "'" +
                                    "           ,'" + i.UpdateDate_EXPROD + "'" +
                                    "           ,'" + i.Update_ID + "');";

                            try
                            {
                                cmd.CommandText = ins;
                                ch = ins;
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception err) { }
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception err)
                {
                    string a = ch;
                }
            }
        }



        public void Sync_T_TimeInOut()
        {
            SqlConnection conn = new SqlConnection(AppConfig.AMSDB);
            SqlCommand cmdSql = new SqlCommand();
           
            using (SQLiteConnection con = new SQLiteConnection(AppConfig.databasepath))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                #region create table
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                string sql ="CREATE TABLE [T_TimeInOut](" +
                            "	[ID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                            "	[Employee_RFID] [varchar](20) NULL," +
                            "	[ScheduleID] [bigint] NULL," +
                            "	[TimeIn] [datetime] NULL," +
                            "	[TimeOut] [datetime] NULL," +
                            "	[LineID] [bigint] NOT NULL," +
                            "	[ProcessID] [bigint] NOT NULL," +
                            "	[DTR_TimeIn] [datetime] NULL," +
                            "	[DTR_TimeOut] [datetime] NULL," +
                            "	[DTR_RefNo] [nvarchar](50) NULL," +
                            "	[CSRef_No] [nvarchar](50) NULL," +
                            "	[CS_ScheduleID] [bigint] NULL" +
                            ")";
                con.Open();
                try
                {
                    cmd = new SQLiteCommand(sql, con);
                    cmd.ExecuteNonQuery();
                }
                catch(Exception err) { }
                con.Close();
                #endregion

            }

           

        }
    }
}
