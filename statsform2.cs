using BitMiracle.LibTiff.Classic;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace DESKTOP_APP
{
    public partial class statsform2 : Form
    {
        Dbconnect1 dbconnect1 = new Dbconnect1();
        SqlCommand cmd;
        SqlDataReader dr,dr1;
        int countNumber = 0;
        int present = 0;

        public object SqlCommand { get; private set; }

        public statsform2()
        {
            InitializeComponent();
            AddItemsToComboBox("SELECT * FROM Manage_meal1", mealcombo);
            mealcombo.Items.Add("All");
        }

        private int count(string text, string tablename,string firstdate,string lastdate, bool date)
        {
            try
            {
                string qurey = "";
                if(date)
                 qurey = "SELECT COUNT(" + text + ") FROM " + tablename + " WHERE date BETWEEN '" + firstdate + "' AND '" + lastdate + "'"; 
                else
                 qurey = "SELECT COUNT (" + text + ") FROM " + tablename;
                cmd = new SqlCommand(qurey, dbconnect1.getCon());
                dbconnect1.OpenCon();
                object resault = cmd.ExecuteScalar();
                if (resault != null && resault != DBNull.Value)
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



        private void apply_btn_Click(object sender, EventArgs e)
        {
            
            

            try
            {
                if (mealcombo.SelectedIndex == -1)
                {
                    MessageBox.Show("Select meal type ", "Warning Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    present = 0;
                    int s = 0;
                    DateTime starttimeselected = startdate.Value;
                    DateTime endtimeselected = enddate.Value;
                    List<string> listbranchs = new List<string>();
                    List<string> listlevels = new List<string>();
                    List<string> listdays = new List<string>();
                    List<string> listsystems = new List<string>();
                    string firstdate = starttimeselected.ToString("MM/dd/yyyy");
                    string lastdate = endtimeselected.ToString("MM/dd/yyyy");
                    int i;
                    if (mealcombo.SelectedItem == "All")
                        i = count("Name", "Manage_meal1", firstdate, lastdate, false);
                    else
                        i = 1;
                   
                    for (int j = 1; j <= i; j++)
                    {
                        string query;
                        query = "SELECT Name FROM (SELECT ROW_NUMBER() OVER(ORDER BY Name) AS RowNum, Name FROM Manage_meal1) AS T WHERE RowNum = " + j.ToString();
                        cmd = new SqlCommand(query, dbconnect1.getCon());
                        dbconnect1.OpenCon();
                        object result = cmd.ExecuteScalar();
                        int savepresent;
                        if (result != null && result != DBNull.Value)
                        {
                            if (i != 1)
                            {
                                savepresent = count("id", result.ToString(), firstdate, lastdate, true);
                            }
                            else
                            {
                                savepresent = count("id", mealcombo.SelectedItem.ToString(), firstdate, lastdate, true);
                            }
                            present += savepresent;
                            if (savepresent > 0) { s++; }
                        }
                        dbconnect1.CloseCon();
                        string query2 = "SELECT * FROM " + result.ToString() + " WHERE date BETWEEN '" + firstdate + "' AND '" + lastdate + "'";
                        SqlCommand cmd1 = new SqlCommand(query2, dbconnect1.getCon());
                        dbconnect1.OpenCon();
                        dr = cmd1.ExecuteReader();
                        while (dr.Read())
                        {
                            listbranchs.Add(dr.GetString(2));
                            listlevels.Add(dr.GetString(3));
                            listsystems.Add(dr.GetString(4));
                            DateTime date = dr.GetDateTime(1);
                            string dayName = date.ToString("dddd");
                            listdays.Add(dayName);


                        }

                        dbconnect1.CloseCon();


                    }

                    string mostRepeatedBranch, mostRepeatedLevel, mostRepeatedDay, mostRepeatedsystem;
                    string mostUnusedBranch, mostUnusedLevel, mostUnusedsystem,mostUnusedDay;
                    if (listbranchs.Count > 0)
                    {
                        mostRepeatedBranch = listbranchs.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
                        mostRepeatedLevel = listlevels.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
                        mostRepeatedDay = listdays.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
                        mostRepeatedsystem = listsystems.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
                        // unused
                        mostUnusedBranch = listbranchs.GroupBy(x => x).OrderBy(x => x.Count()).First().Key;
                        mostUnusedLevel = listlevels.GroupBy(x => x).OrderBy(x => x.Count()).First().Key;
                        mostUnusedsystem = listsystems.GroupBy(x => x).OrderBy(x => x.Count()).First().Key;
                        mostUnusedDay = listdays.GroupBy(x => x).OrderBy(x => x.Count()).First().Key;
                        // if they are the same (one system)
                        if(mostRepeatedBranch == mostUnusedBranch) { mostUnusedBranch = "Only One Branch Has been marked"; }
                        if (mostRepeatedLevel == mostUnusedLevel) { mostUnusedLevel = "Only One Level Has been marked"; }
                        if (mostRepeatedsystem == mostUnusedsystem) { mostUnusedsystem = "Only One system Has been marked"; }
                        if (mostRepeatedDay == mostUnusedDay) { mostUnusedDay = "Only One Day Has been marked"; }



                    }
                    else
                    {
                        mostRepeatedBranch = "No branch";
                        mostRepeatedLevel = "No level";
                        mostRepeatedDay = "No day";
                        mostRepeatedsystem = "No system";
                        mostUnusedBranch = "No Branch";
                        mostUnusedDay = "No Day";
                        mostUnusedLevel = "No Level";
                        mostUnusedsystem = "No system";
                    }
                    // get the poupler dishe 
                    string qurey3 = "SELECT Name FROM meal_list WHERE Day=@day";
                    SqlCommand cmd3 = new SqlCommand(qurey3, dbconnect1.getCon());
                    dbconnect1.OpenCon();
                    cmd3.Parameters.AddWithValue("@day", mostRepeatedDay);
                    object result2 = cmd3.ExecuteScalar();
                    string mostPopulardishe;
                    if (result2 != null)
                    {
                        mostPopulardishe = result2.ToString();
                    }
                    else
                    {
                        mostPopulardishe = "No dishes";
                    }
                    dbconnect1.CloseCon();


                    // FILLING THE LABLES 

                    overallattendance.Text = present.ToString();
                    if (s != 0)
                        avgattendance.Text = (present / s).ToString();
                    else
                        avgattendance.Text = "0";

                    mostattendingbranch.Text = mostRepeatedBranch;
                    mostattendinglevel.Text = mostRepeatedLevel;
                    dayofhighest.Text = mostRepeatedDay;
                    mostattendingsystem.Text = mostRepeatedsystem;
                    populardishes.Text = mostPopulardishe;
                    leastattendinglevel.Text = mostUnusedLevel;
                    leastattendingsystem.Text = mostUnusedsystem;
                    lestattendingbranch.Text = mostUnusedBranch;
                    daywithlowestattendance.Text = mostUnusedDay;




                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2HtmlLabel17_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel11_Click(object sender, EventArgs e)
        {

        }
        Bitmap bmp;

        private void PrintCurrentWindow()
        {
            // Get the current window handle
            IntPtr hwnd = this.Handle;

            // Get the current window's size and location
            RECT rect;
            GetWindowRect(hwnd, out rect);

            // Calculate the width and height of the window
            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            // Calculate the new rectangle for the bottom half of the window
            int newTop = rect.bottom - height / 2;
            RECT newRect = new RECT() { left = rect.left, top = newTop, right = rect.right, bottom = rect.bottom };

            // Create a bitmap to capture the window
            Bitmap bmp = new Bitmap(width - 120, height + 200 );

            // Capture the bottom half of the window into the bitmap
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(newRect.left, newRect.top, 0, 0, new Size(width, height / 2));
            }

            // Show the print dialog
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                // Print the bitmap
                printDocument1.PrinterSettings = printDialog.PrinterSettings;
                printDocument1.PrintPage += (s, e) => e.Graphics.DrawImage(bmp, e.MarginBounds);
                printDocument1.Print();
            }
        }

        // Declare the RECT structure and the GetWindowRect function from user32.dll
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            PrintCurrentWindow();
        }

        private void Unfilterbtn_Click(object sender, EventArgs e)
        {
            
                present = 0;
                int s = 0;
                List<string> listbranchs = new List<string>();
                List<string> listlevels = new List<string>();
                List<string> listdays = new List<string>();
                List<string> listsystems = new List<string>();
                int i;
            i = count("Name", "Manage_meal1", "", "", false);

            for (int j = 1; j <= i; j++)
                {
                    string query;
                    query = "SELECT Name FROM (SELECT ROW_NUMBER() OVER(ORDER BY Name) AS RowNum, Name FROM Manage_meal1) AS T WHERE RowNum = " + j.ToString();
                    cmd = new SqlCommand(query, dbconnect1.getCon());
                    dbconnect1.OpenCon();
                    object result = cmd.ExecuteScalar();
                    int savepresent;
                    if (result != null && result != DBNull.Value)
                    {
                            savepresent = count("id", result.ToString(), "", "", false);
                             present += savepresent;
                             if (savepresent > 0) { s++; }
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
                        listsystems.Add(dr.GetString(4));
                        DateTime date = dr.GetDateTime(1);
                        string dayName = date.ToString("dddd");
                        listdays.Add(dayName);


                    }

                    dbconnect1.CloseCon();


                }

                string mostRepeatedBranch, mostRepeatedLevel, mostRepeatedDay, mostRepeatedsystem;
                string mostUnusedBranch, mostUnusedLevel, mostUnusedsystem, mostUnusedDay;
                if (listbranchs.Count > 0)
                {
                    mostRepeatedBranch = listbranchs.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
                    mostRepeatedLevel = listlevels.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
                    mostRepeatedDay = listdays.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
                    mostRepeatedsystem = listsystems.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
                    // unused
                    mostUnusedBranch = listbranchs.GroupBy(x => x).OrderBy(x => x.Count()).First().Key;
                    mostUnusedLevel = listlevels.GroupBy(x => x).OrderBy(x => x.Count()).First().Key;
                    mostUnusedsystem = listsystems.GroupBy(x => x).OrderBy(x => x.Count()).First().Key;
                    mostUnusedDay = listdays.GroupBy(x => x).OrderBy(x => x.Count()).First().Key;
                    // if they are the same (one system)
                    if (mostRepeatedBranch == mostUnusedBranch) { mostUnusedBranch = "Only One Branch Has been marked"; }
                    if (mostRepeatedLevel == mostUnusedLevel) { mostUnusedLevel = "Only One Level Has been marked"; }
                    if (mostRepeatedsystem == mostUnusedsystem) { mostUnusedsystem = "Only One system Has been marked"; }
                    if (mostRepeatedDay == mostUnusedDay) { mostUnusedDay = "Only One Day Has been marked"; }



                }
                else
                {
                    mostRepeatedBranch = "No branch";
                    mostRepeatedLevel = "No level";
                    mostRepeatedDay = "No day";
                    mostRepeatedsystem = "No system";
                    mostUnusedBranch = "No Branch";
                    mostUnusedDay = "No Day";
                    mostUnusedLevel = "No Level";
                    mostUnusedsystem = "No system";
                }
                // get the poupler dishe 
                string qurey3 = "SELECT Name FROM meal_list WHERE Day=@day";
                SqlCommand cmd3 = new SqlCommand(qurey3, dbconnect1.getCon());
                dbconnect1.OpenCon();
                cmd3.Parameters.AddWithValue("@day", mostRepeatedDay);
                object result2 = cmd3.ExecuteScalar();
                string mostPopulardishe;
                if (result2 != null)
                {
                    mostPopulardishe = result2.ToString();
                }
                else
                {
                    mostPopulardishe = "No dishes";
                }
                dbconnect1.CloseCon();


                // FILLING THE LABLES 

                overallattendance.Text = present.ToString();
                if (s != 0)
                    avgattendance.Text = (present / s).ToString();
                else
                    avgattendance.Text = "0";

                mostattendingbranch.Text = mostRepeatedBranch;
                mostattendinglevel.Text = mostRepeatedLevel;
                dayofhighest.Text = mostRepeatedDay;
                mostattendingsystem.Text = mostRepeatedsystem;
                populardishes.Text = mostPopulardishe;
                leastattendinglevel.Text = mostUnusedLevel;
                leastattendingsystem.Text = mostUnusedsystem;
                lestattendingbranch.Text = mostUnusedBranch;
                daywithlowestattendance.Text = mostUnusedDay;




            
        }

        public void AddItemsToComboBox(string query, ComboBox comboBox)
        {
            var command = new SqlCommand(query, dbconnect1.getCon());
            dbconnect1.OpenCon();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    comboBox.Items.Add(reader.GetString(0));
                }
            }
            dbconnect1.CloseCon();

        }
    }
}
