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
    public partial class login : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public login()
        {
            InitializeComponent();
        }
        bool test = false;
        string saveusername;
        private void testtheaccount()
        {
            string username = usernametextbox.Text;
            saveusername = username;
            string selectQurey = "SELECT * FROM login_tabl WHERE Username = @username";
            SqlCommand cm = new SqlCommand(selectQurey, dbconnect.getCon());
            cm.Parameters.AddWithValue("@username", username);
            dbconnect.OpenCon();
            dr = cm.ExecuteReader();
            if (dr.HasRows)
            {
                string password = passwordtextbox.Text;
                string selectQurey1 = "SELECT * FROM login_tabl WHERE Password = @password";
                SqlCommand cm1 = new SqlCommand(selectQurey1, dbconnect.getCon());
                cm1.Parameters.AddWithValue("@password", password);
                dr.Close(); // close the first SqlDataReader before executing the next query
                dr = cm1.ExecuteReader();
                if (dr.HasRows)
                {
                    test = true;
                }
                else
                {
                    MessageBox.Show("Password invalid ", "Warning Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cm1.Dispose(); // dispose of the SqlCommand
            }
            else
            {
                MessageBox.Show("Username invalid ", "Warning Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cm.Dispose(); // dispose of the SqlCommand
            dr.Close(); // close the SqlDataReader
        }



        private void bunifuProgressBar1_progressChanged(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel1_Click_1(object sender, EventArgs e)
        {

        }

        private void gunaLabel1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
           
            testtheaccount();
            if (test)
            {
                MessageBox.Show("Login successful!");
                CanteenApp f = new CanteenApp(saveusername);
                f.Show();
                this.Hide();

            }
        }

        private void hover_exit(object sender, EventArgs e)
        {
            
        }

        private void leave_login_MouseEnter(object sender, EventArgs e)
        {
            leave_login.ForeColor = Color.Red;
        }

        private void leave_login_MouseLeave(object sender, EventArgs e)
        {
            leave_login.ForeColor = Color.White;
        }

        private void gunaTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {
            
        }

        private void gunaLabel1_Click_1(object sender, EventArgs e)
        {
            backupcode b = new backupcode();
            b.ShowDialog();
            this.Close();
        }
    }
}
