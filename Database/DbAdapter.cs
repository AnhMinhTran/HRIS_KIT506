using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows; //for generating a MessageBox upon encountering an error
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using HRIS_KIT506.Teaching;
using MySql.Data.MySqlClient;
using MySql.Data.Types;


namespace HRIS_KIT506.Database
{
    abstract class DbAdapter
    {
        private static bool ReportingErrors = false;
        private const string db = "kit206";
        private const string user = "kit206";
        private const string pass = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";

        private static MySqlConnection conn = null;

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Creates and returns (but does not open) the connection to the database.
        /// </summary>
        private static MySqlConnection GetConnection()
        {
            if (conn == null)
            {
                //Note: This approach is not thread-safe
                string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
        }


        //Load all staff in db
        public static List<Staff> LoadAllStaff()
        {
            List<Staff> staff = new List<Staff>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;


            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand
                    (
                    "select id, title, given_name, family_name, campus, phone, room, email, category, photo from staff", conn
                    );
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    byte[] bruh = Encoding.ASCII.GetBytes(rdr.GetString(9));

                    staff.Add(new Staff
                    {
                        ID = rdr.GetInt32(0),
                        Title = rdr.GetString(1),
                        GivenName = rdr.GetString(2),
                        FamilyName = rdr.GetString(3),
                        Campus = ParseEnum<Campus>(rdr.GetString(4)),
                        Phone = rdr.GetString(5),
                        Room = rdr.GetString(6),
                        Email = rdr.GetString(7),
                        Category = ParseEnum<Category>(rdr.GetString(8)),
                        Image = rdr.GetString(9)
                    }) ;
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading staff", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return staff;
        }

        //Load specific staff in db
        public static List<Staff> LoadStaff(int id)
        {
            List<Staff> staff = new List<Staff>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;


            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand
                    (
                    "select id, title, given_name, family_name, campus, phone, room, email, category, photo from staff where id=?id", conn
                    );
                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    byte[] bruh = Encoding.ASCII.GetBytes(rdr.GetString(9));

                    staff.Add(new Staff
                    {
                        ID = rdr.GetInt32(0),
                        Title = rdr.GetString(1),
                        GivenName = rdr.GetString(2),
                        FamilyName = rdr.GetString(3),
                        Campus = ParseEnum<Campus>(rdr.GetString(4)),
                        Phone = rdr.GetString(5),
                        Room = rdr.GetString(6),
                        Email = rdr.GetString(7),
                        Category = ParseEnum<Category>(rdr.GetString(8)),
                        Image = rdr.GetString(9)
                    });
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading staff", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return staff;
        }

        //Load consultation hours in db
        public static List<Consultation> LoadConsultationItems(int id)
        {
            List<Consultation> work = new List<Consultation>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select day, start, end from consultation where staff_id=?id", conn);
                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    work.Add(new Consultation
                    {
                        Day = ParseEnum<DayOfWeek>(rdr.GetString(0)),
                        Start = rdr.GetTimeSpan(1),
                        End = rdr.GetTimeSpan(2)
                    });
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading roster items", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return work;
        }

        //load selected staff teaching units in db
        public static List<Unit> LoadStaffUnit(int id)
        {
            List<Unit> teach = new List<Unit>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand
                    ("select distinct class.unit_code, unit.title from class inner join unit on class.unit_code = unit.code where class.staff=?id", conn);
                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    teach.Add(new Unit
                    {
                        Code = rdr.GetString(0),
                        Title = rdr.GetString(1),
                    });
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading unit", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return teach;
        }

        //load all units in db
        public static List<Unit> LoadAllUnit()
        {
            List<Unit> unit = new List<Unit>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select code, title, coordinator from unit", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    unit.Add(new Unit
                    {
                        Code = rdr.GetString(0),
                        Title = rdr.GetString(1),
                        Coordinator = rdr.GetInt32(2)
                    });
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading units", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return unit;
        }

        //load classes of selectced unit in db
        public static List<Class> LoadClasses(string code)
        {
            List<Class> course = new List<Class>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand
                    (
                    "select class.day, class.start, class.end, class.type, class.room, class.campus, class.staff, " +
                    "staff.given_name, staff.family_name, staff.title " +
                    "from class inner join staff on class.staff = staff.id " +
                    "where unit_code=?code", conn
                    );
                cmd.Parameters.AddWithValue("code", code);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    course.Add(new Class
                    {
                        Day = ParseEnum<DayOfWeek>(rdr.GetString(0)),
                        Start = rdr.GetTimeSpan(1),
                        End = rdr.GetTimeSpan(2),
                        Type = ParseEnum<Teaching.Type>(rdr.GetString(3)),
                        Room = rdr.GetString(4),
                        Campus = ParseEnum<Campus>(rdr.GetString(5)),
                        StaffID = rdr.GetInt32(6),
                        StaffName = rdr.GetString(7) + " " + rdr.GetString(8),
                    });
                    
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading classes", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return course;
        }


        /// <summary>
        /// In a more complete application this error would be logged to a file
        /// and the error reported back to the original caller, who is closer
        /// to the GUI and hence better able to produce the error message box
        /// (which would not show the actual error details like this does).
        /// </summary>
        private static void ReportError(string msg, Exception e)
        {
            if (ReportingErrors)
            {
                MessageBox.Show("An error occurred while " + msg + ". Try again later.\n\nError Details:\n" + e,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
