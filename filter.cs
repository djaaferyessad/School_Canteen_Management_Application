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

namespace DESKTOP_APP
{
    
    public partial class filter : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        private ComboBox comboboxref;
        private ComboBox comboboxref2;
        private ComboBox comboboxref3;
        private ComboBox comboboxref4;
        private DataGridView dt;
        public List<string> ComboBoxItems { get; set; }
        public filter(ComboBox combobox , ComboBox combobox2 , ComboBox combobox3, ComboBox combobox4,ref DataGridView table)
        {
            InitializeComponent();
           this.comboboxref = combobox;
            this.comboboxref2 = combobox2;
            this.comboboxref3 = combobox3;
            this.comboboxref4 = combobox4;
            comboBox1.Items.AddRange(comboboxref.Items.Cast<string>().ToArray());
            ComboBox2.Items.AddRange(comboboxref2.Items.Cast<string>().ToArray());
            ComboBox3.Items.AddRange(comboboxref3.Items.Cast<string>().ToArray());
            ComboBox4.Items.AddRange(comboboxref4.Items.Cast<string>().ToArray());
            dt = table;
            


        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void apply_btn_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 && ComboBox2.SelectedIndex == -1 &&
    ComboBox3.SelectedIndex == -1 && ComboBox4.SelectedIndex == -1)
            {
                // none of the ComboBoxes is selected
                return;
            }
            /* 
              cm.Parameters.AddWithValue("@" + parameterName3, parameterValue3);
            cm.Parameters.AddWithValue("@" + parameterName4, parameterValue4);
            string parameterValue3 = "";
            string parameterValue4 = "";
             string parameterName3 = "";
            string parameterName4 = "";

             */
            int i = 0;
            dt.Rows.Clear();
            string selectQurey = "SELECT * FROM Std_list WHERE ";
            string parameterValue = "";
            string parameterValue2 = "";
            string parameterValue3 = "";
            string parameterValue4 = "";
            string parameterName = "";
            string parameterName2 = "";
            string parameterName3 = "";
            string parameterName4 = "";
            bool onlyone = false;
            bool onlytwo = false;
            bool onlythree = false;
            bool allofthem = false;
            // filter if just one selected 
            if (comboBox1.SelectedIndex != -1)
            {
                parameterName = "Year";
                parameterValue = comboBox1.Text;
                onlyone = true;
            }
            else if (ComboBox2.SelectedIndex != -1)
            {
                parameterName = "Branch";
                parameterValue = ComboBox2.Text;
                onlyone = true;
            }
            else if (ComboBox3.SelectedIndex != -1)
            {
                parameterName = "GroupNum";
                parameterValue = ComboBox3.Text;
                onlyone = true;
            }
            else if (ComboBox4.SelectedIndex != -1)
            {
                parameterName = "School_system";
                parameterValue = ComboBox4.Text;
                onlyone = true;
            }
            // year and another one just two selected with year 
            if (comboBox1.SelectedIndex != -1 && ComboBox2.SelectedIndex != -1)
            {
                parameterName = "Year";
                parameterValue = comboBox1.Text;
                parameterName2 = "Branch";
                parameterValue2 = ComboBox2.Text;
                onlyone = false;
                onlytwo = true;
            }
            if (comboBox1.SelectedIndex != -1 && ComboBox3.SelectedIndex != -1)
            {
                parameterName = "Year";
                parameterValue = comboBox1.Text;
                parameterName2 = "GroupNum";
                parameterValue2 = ComboBox3.Text;
                onlyone = false;
                onlytwo = true;
            }
            if (comboBox1.SelectedIndex != -1 && ComboBox4.SelectedIndex != -1)
            {
                parameterName = "Year";
                parameterValue = comboBox1.Text;
                parameterName2 = "School_system";
                parameterValue2 = ComboBox4.Text;
                onlyone = false;
                onlytwo = true;
            }
            // three selected with year 
            if (comboBox1.SelectedIndex != -1 && ComboBox2.SelectedIndex != -1 && ComboBox3.SelectedIndex != -1)
            {
                parameterName = "Year";
                parameterValue = comboBox1.Text;
                parameterName2 = "Branch";
                parameterValue2 = ComboBox2.Text;
                parameterName3 = "GroupNum";
                parameterValue3 = ComboBox3.Text;
                onlyone = false;
                onlytwo = false;
                onlythree = true;
            }
            if (comboBox1.SelectedIndex != -1 && ComboBox2.SelectedIndex != -1 && ComboBox4.SelectedIndex != -1)
            {
                parameterName = "Year";
                parameterValue = comboBox1.Text;
                parameterName2 = "Branch";
                parameterValue2 = ComboBox2.Text;
                parameterName3 = "School_system";
                parameterValue3 = ComboBox4.Text;
                onlyone = false;
                onlytwo = false;
                onlythree = true;
            }
            if (comboBox1.SelectedIndex != -1 && ComboBox3.SelectedIndex != -1 && ComboBox4.SelectedIndex != -1)
            {
                parameterName = "Year";
                parameterValue = comboBox1.Text;
                parameterName2 = "GroupNum";
                parameterValue2 = ComboBox3.Text;
                parameterName3 = "School_system";
                parameterValue3 = ComboBox4.Text;
                onlyone = false;
                onlytwo = false;
                onlythree = true;
            }
            // only two selected with Branch
            if (ComboBox2.SelectedIndex != -1 && ComboBox3.SelectedIndex != -1)
            {
                parameterName = "Branch";
                parameterValue = ComboBox2.Text;
                parameterName2 = "GroupNum";
                parameterValue2 = ComboBox3.Text;
                onlyone = false;
                onlytwo = true;
            }

            if (ComboBox2.SelectedIndex != -1 && ComboBox4.SelectedIndex != -1)
            {
                parameterName = "Branch";
                parameterValue = ComboBox2.Text;
                parameterName2 = "School_system";
                parameterValue2 = ComboBox4.Text;
                onlyone = false;
                onlytwo = true;
            }
            // only two selected with group
            if (ComboBox3.SelectedIndex != -1 && ComboBox4.SelectedIndex != -1)
            {
                parameterName = "GroupNum";
                parameterValue = ComboBox3.Text;
                parameterName2 = "School_system";
                parameterValue2 = ComboBox4.Text;
                onlyone = false;
                onlytwo = true;
            }
            // if all of them selected 
            if (comboBox1.SelectedIndex != -1 && ComboBox3.SelectedIndex != -1 && ComboBox4.SelectedIndex != -1 && ComboBox2.SelectedIndex != -1)
            {
                parameterName = "Year";
                parameterValue = comboBox1.Text;
                parameterName2 = "GroupNum";
                parameterValue2 = ComboBox3.Text;
                parameterName3 = "School_system";
                parameterValue3 = ComboBox4.Text;
                parameterName4 = "Branch";
                parameterValue4 = ComboBox2.Text;
                onlyone = false;
                onlytwo = false;
                onlythree = false;
                allofthem = true;
            }

            if (onlyone)
                selectQurey += parameterName + " = @" + parameterName;

            if (onlytwo)
            {
                selectQurey += parameterName + " = @" + parameterName + " AND " + parameterName2 + " = @" + parameterName2;
            }

            if (onlythree)
            {
                selectQurey += parameterName + " = @" + parameterName + " AND " + parameterName2 + " = @" + parameterName2 + " AND " + parameterName3 + " = @" + parameterName3;  
            }
            if (allofthem)
            {
                selectQurey += parameterName + " = @" + parameterName + " AND " + parameterName2 + " = @" + parameterName2 + " AND " + parameterName3 + " = @" + parameterName3 + " AND " + parameterName4 + " = @" + parameterName4;
            }
        



            SqlCommand   cm = new SqlCommand(selectQurey, dbconnect.getCon());
            cm.Parameters.AddWithValue("@" + parameterName, parameterValue);

            if (onlytwo)
            {
                cm.Parameters.AddWithValue("@" + parameterName2, parameterValue2);
            }
            if (onlythree)
            {
                cm.Parameters.AddWithValue("@" + parameterName2, parameterValue2);
                cm.Parameters.AddWithValue("@" + parameterName3, parameterValue3);
            }
            if(allofthem)
            {
                cm.Parameters.AddWithValue("@" + parameterName2, parameterValue2);
                cm.Parameters.AddWithValue("@" + parameterName3, parameterValue3);
                cm.Parameters.AddWithValue("@" + parameterName4, parameterValue4);
            }

            dbconnect.OpenCon();
            dr = cm.ExecuteReader();

            while (dr.Read())
            {
                i++;
                dt.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
            }

            dr.Close();
            dbconnect.CloseCon();
            this.Close();


        }
    }
}
