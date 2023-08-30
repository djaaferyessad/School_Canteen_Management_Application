using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace DESKTOP_APP
{
    public partial class statsform : Form
    {
        Dbconnect1 dbconnect1 = new Dbconnect1();
        SqlCommand cmd;
        SqlDataReader dr,dr1;
        int countNumber = 0;
        int avgpresent = 0;
        public statsform()
        {
            InitializeComponent();
            getinfos();
        }

        private int count(string text , string tablename)
        {
            try
            {
                string qurey = "SELECT COUNT (" + text + ") FROM " + tablename;
                cmd = new SqlCommand(qurey, dbconnect1.getCon());
                dbconnect1.OpenCon();
                object resault = cmd.ExecuteScalar();
                if(resault != null && resault != DBNull.Value) 
                {
                    countNumber = Convert.ToInt32(resault);
                    return countNumber;
                }
                dbconnect1.CloseCon();
                return 0;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        private void morebtn_Click(object sender, EventArgs e)
        {
            statsform2 f = new statsform2();
           f.ShowDialog();

        }

        private void getinfos()
        {
            List<string> listbranchs = new List<string>();
            List<string> listlevels = new List<string>();
            List<string> listdays = new List<string>();
            numberstudent.Text = count("id", "Std_list").ToString();
            numbergr.Text = count("Name", "Manage_std2").ToString();
            numberbr.Text = count("Name", "Manage_std1").ToString();
            numberro.Text = count("Name","Manage_meal2").ToString();
            numberme.Text = count("Id", "meal_list").ToString();
            numbergrlv.Text = count("Name","Manage_std3").ToString() ;
            numbersy.Text = count("Name", "Manage_std4").ToString() ;
            int i = count("Name", "Manage_meal1");
            int s = 0;
            for (int j=1; j <= i; j++) 
            {
                string query = "SELECT Name FROM (SELECT ROW_NUMBER() OVER(ORDER BY Name) AS RowNum, Name FROM Manage_meal1) AS T WHERE RowNum = " + j.ToString();
                cmd = new SqlCommand(query, dbconnect1.getCon());
                dbconnect1.OpenCon();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    int saveavg = count("id", result.ToString());
                    avgpresent += count("id", result.ToString());
                    if (saveavg > 0) { s++; }    
                }
                dbconnect1.CloseCon();
                string query2 = "SELECT * FROM " + result.ToString() ;
                SqlCommand cmd1 = new SqlCommand(query2, dbconnect1.getCon());
                dbconnect1.OpenCon();
                dr = cmd1.ExecuteReader();
                while (dr.Read())
                {
                    listbranchs.Add(dr.GetString(2));
                    listlevels.Add(dr.GetString(3));
                    DateTime date = dr.GetDateTime(1);
                    string dayName = date.ToString("dddd");
                    listdays.Add(dayName);


                }
                dbconnect1.CloseCon();

            }
            string mostRepeatedBranch,mostRepeatedLevel,mostRepeatedDay;
            if (listbranchs.Count > 0)
            {
               mostRepeatedBranch = listbranchs.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
                mostRepeatedLevel = listlevels.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
                mostRepeatedDay = listdays.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;

            }
            else
            {
                mostRepeatedBranch = "No branch";
                mostRepeatedLevel = "No level";
                mostRepeatedDay = "No day";
            }
            if (s != 0)
                numberAverage.Text = (avgpresent / s).ToString();
            else 
                numberAverage.Text = "0";

            nameattendingbranch.Text = mostRepeatedBranch;
            nameattendinglevel.Text = mostRepeatedLevel;
            nameattendance.Text = mostRepeatedDay;
            
        }
     
    }
}
