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
    
    public partial class staffPass : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        string username;
        public staffPass(string user)
        {
            InitializeComponent();
            username = user;
            usernamelabel.Text = user;
        }

        private void staffPass_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string selectqurey = "SELECT Password FROM login_tabl WHERE Username = @Username ";
            SqlCommand cmd = new SqlCommand(selectqurey, dbconnect.getCon());
            cmd.Parameters.AddWithValue("@Username", username);
            dbconnect.OpenCon();
            object resault = cmd.ExecuteScalar();

            if(resault != null)
            {
                string text = resault.ToString().Trim();
                if(passwordbox1.TextLength < 8)
                {
                    MessageBox.Show("Password have less then 8 characters.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (passwordbox1.Text == passwordbox2.Text)
                {
                    if(currentpassword.Text == text)
                    {
                        SqlCommand command = new SqlCommand("UPDATE login_tabl SET Password=@password WHERE Username LIKE @username ", dbconnect.getCon());
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@password", passwordbox1.Text);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Password has been modified successfully");

                    }
                    else
                    { MessageBox.Show("Password Wrong.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                } 
                else
                {
                    MessageBox.Show("Passwords do not match.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                dbconnect.CloseCon();
            }
        }
    }
}
