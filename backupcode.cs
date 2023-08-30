using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_APP
{
    public partial class backupcode : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm;
        SqlDataReader dr;
        public backupcode()
        {
            InitializeComponent();
        }

        private void bunifuCustomLabel1_Click(object sender, EventArgs e)
        {

        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            int code = Convert.ToInt32(usernametextbox.Text);
            string qurey = "SELECT * FROM login_tabl WHERE Backupcode = @code";
            SqlCommand cm = new SqlCommand(qurey, dbconnect.getCon());
            cm.Parameters.AddWithValue("@code", code);
            dbconnect.OpenCon();
            dr = cm.ExecuteReader();
            if(dr.Read())
            {
                MessageBox.Show("Login Successfully");
                string role = dr.GetString(2).Trim();
                CanteenApp f = new CanteenApp(role);
                f.Show();
                this.Close();
                
            }
            else
            {
                MessageBox.Show("Back up code doesn't exists" , "waring" , MessageBoxButtons.OK , MessageBoxIcon.Error);
                usernametextbox.Text = "";
            }
            dbconnect.CloseCon();
        }

        private void leave_login_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
    }
}
