using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeSheetEmployee
{
    public partial class Form1 : Form
    {
        private DateTime startdate;
        private DateTime enddate;
        private int employeeid;
        private int projectid;
        private int tasktypeid;
        private int statusid;

        public Form1()
        {
            InitializeComponent();
            startdate = DateTime.Now.Date;
            enddate = DateTime.Now.Date;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) => startdate = dateTimePicker1.Value.Date;

        private void btn_Executar_Click(object sender, EventArgs e)
        {            
            if (string.IsNullOrEmpty(txt_employeeid.Text) || string.IsNullOrEmpty(txt_projectid.Text) || string.IsNullOrEmpty(txt_tasktypeid.Text) || string.IsNullOrEmpty(txt_statusid.Text))
                return;

            employeeid = Convert.ToInt32(txt_employeeid.Text);
            projectid = Convert.ToInt32(txt_projectid.Text);
            tasktypeid = Convert.ToInt32(txt_tasktypeid.Text);
            statusid = Convert.ToInt32(txt_statusid.Text);


            //ExecuteInsertEmployeeHours(employeeid,startdate , statusid , projectid , tasktypeid, Convert.ToInt32((enddate - startdate).TotalDays));


            if (!ExecuteInsert_OSUSR_KWM_TIMESHEET3(employeeid, startdate, statusid)) return;
            string timesheetid = SelectID_OSUSR_KWM_TIMESHEET3(employeeid, startdate);
            if (!ExecuteInsert_OSUSR_KWM_TIMESHEETLINE3(Convert.ToInt32(timesheetid), projectid, tasktypeid)) return;
            string timesheetlineid = SelectID_OSUSR_KWM_TIMESHEETLINE3(Convert.ToInt32(timesheetid), projectid, tasktypeid);
            if (!ExecuteInsert_OSUSR_KWM_TIMESHEETITEM3(Convert.ToInt32(timesheetlineid), startdate, Convert.ToInt32((enddate - startdate).TotalDays))) return;
        }


        private string SelectID_OSUSR_KWM_TIMESHEET3(int employeeid, DateTime date)
        {
            String connectionstring = ConfigurationManager.ConnectionStrings["stringconnect"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionstring);
            try
            {
                SqlCommand cmd = new SqlCommand("select ID from  OSUSR_KWM_TIMESHEET3 where EMPLOYEEID = @employeeid and STARTDATE = @startdate ", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@employeeid", employeeid);
                cmd.Parameters.AddWithValue("@startdate", date.Date);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader["ID"].ToString();
                    }
                }
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }



        private string SelectID_OSUSR_KWM_TIMESHEETLINE3(int timesheetid, int projectid, int tasktypeid)
        {
            String connectionstring = ConfigurationManager.ConnectionStrings["stringconnect"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionstring);
            try
            {
                SqlCommand cmd = new SqlCommand("select ID from OSUSR_KWM_TIMESHEETLINE3 where TIMESHEETID = @timesheetid and projectid = @projectid and tasktypeid = @tasktypeid", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@timesheetid", timesheetid);
                cmd.Parameters.AddWithValue("@projectid", projectid);
                cmd.Parameters.AddWithValue("@tasktypeid", tasktypeid);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader["ID"].ToString();
                    }
                }
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }


        private bool ExecuteInsert_OSUSR_KWM_TIMESHEET3(int employeeid, DateTime startdate, int statusid)
        {
            String connectionstring = ConfigurationManager.ConnectionStrings["stringconnect"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionstring);
            try
            {
                SqlCommand cmd = new SqlCommand("insert_OSUSR_KWM_TIMESHEET3", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@employeeid", SqlDbType.Int).Value = employeeid;
                cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = startdate.Date;
                cmd.Parameters.Add("@statuid", SqlDbType.Int).Value = statusid;

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("sucesso");
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }



        private bool ExecuteInsert_OSUSR_KWM_TIMESHEETLINE3(int timesheetid, int projectid, int tasktypeid)
        {
            String connectionstring = ConfigurationManager.ConnectionStrings["stringconnect"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionstring);
            try
            {
                SqlCommand cmd = new SqlCommand("insert_OSUSR_KWM_TIMESHEETLINE3", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@timesheetid", SqlDbType.Int).Value = timesheetid;
                cmd.Parameters.Add("@projectid", SqlDbType.Int).Value = projectid;
                cmd.Parameters.Add("@tasktypeid", SqlDbType.Int).Value = tasktypeid;

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("sucesso");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }


        private bool ExecuteInsertEmployeeHours(int employeeid,DateTime startdate, int statuid, int projectid, int tasktypeid , int numberofdays)
        {
            String connectionstring = ConfigurationManager.ConnectionStrings["stringconnect"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionstring);
            try
            {
                SqlCommand cmd = new SqlCommand("insert_EmployeeHours", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@employeeid", SqlDbType.Int).Value = employeeid;
                cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = startdate;
                cmd.Parameters.Add("@statuid", SqlDbType.Int).Value = statuid;
                cmd.Parameters.Add("@projectid", SqlDbType.Int).Value = projectid;
                cmd.Parameters.Add("@tasktypeid", SqlDbType.Int).Value = tasktypeid;
                cmd.Parameters.Add("@numberofdays", SqlDbType.Int).Value = numberofdays;

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("sucesso");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }







        private bool ExecuteInsert_OSUSR_KWM_TIMESHEETITEM3(int timesheetlineid, DateTime date , int numberofdays)
        {

            String connectionstring = ConfigurationManager.ConnectionStrings["stringconnect"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionstring);
            try
            {
                SqlCommand cmd = new SqlCommand("insert_OSUSR_KWM_TIMESHEETITEM3", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@timesheetlineid", SqlDbType.Int).Value = timesheetlineid;
                cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = date.Date;
                cmd.Parameters.Add("@numberofdays", SqlDbType.Int).Value = numberofdays;

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("sucesso");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }

        }

        private string TestDb()
        {
            String connectionstring = ConfigurationManager.ConnectionStrings["stringconnect"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionstring);
            try
            {
                SqlCommand cmd = new SqlCommand("select * from  OSUSR_KWM_TIMESHEET3", con);
                cmd.CommandType = CommandType.Text;               

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    bool c = reader.HasRows;
                    if (reader.Read())
                    {
                        return reader["ID"].ToString();
                    }
                }
                return string.Empty;
            }catch(Exception ex)
            {
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            enddate = dateTimePicker2.Value.Date;
        }
    }
}
