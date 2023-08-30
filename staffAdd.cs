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
    public partial class staffAdd : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm;
        SqlDataReader dr;
        public staffAdd()
        {
            InitializeComponent();
        }
        private void reste()
        { 
            Addressbox.Text = "";
            nameBox.Text = "";
            Surnamebox.Text = "";
            phonebox.Text = "";
            designationbox.Text = "";
            usernamebox1.Text = "";
            passwordbox1.Text = "";
            passwordbox2.Text = "";
        }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void staffAdd_Load(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel9_Click(object sender, EventArgs e)
        {

        }

        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void add_btn_Click(object sender, EventArgs e)
        {
            if(Addressbox.Text != "" && nameBox.Text != "" && Surnamebox.Text != "" && phonebox.Text != "" && designationbox.Text != "" && usernamebox1.Text != "" && passwordbox1.Text != "" && passwordbox2.Text != "")
            {
                try
                {
                    if(phonebox.Text.Length == 10 )
                    {
                        if(passwordbox1.TextLength < 8)
                        {
                            MessageBox.Show("Passwords have less then 8 characters", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if(passwordbox1.Text != passwordbox2.Text)
                        {
                            MessageBox.Show("Passwords do not match.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            int id = 0;
                            Random rnd = new Random();
                            int randomNumber = rnd.Next(100000, 999999);
                            string lastIdQuery = "SELECT TOP 1 id FROM login_tabl ORDER BY id DESC";
                            SqlCommand lastIdCommand = new SqlCommand(lastIdQuery, dbconnect.getCon());
                            dbconnect.OpenCon();
                            object lastIdResult = lastIdCommand.ExecuteScalar();
                            if (lastIdResult != null && !DBNull.Value.Equals(lastIdResult))
                            {
                                id = (int)lastIdResult + 1;
                            }
                            dbconnect.CloseCon();
                            string insertQurey = "INSERT INTO login_tabl (Username,Password,Role,id,phone,Address,Name,Surname,Backupcode) VALUES (@Username,@Password,@Role,@id,@phone,@Address,@Name,@Surname,@backupcode)";
                            SqlCommand command = new SqlCommand(insertQurey, dbconnect.getCon());
                            command.CommandType = CommandType.Text;
                            command.Parameters.AddWithValue("@Username", usernamebox1.Text);
                            command.Parameters.AddWithValue("@Password", passwordbox1.Text);
                            command.Parameters.AddWithValue("@Role", designationbox.Text);
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@phone", phonebox.Text);
                            command.Parameters.AddWithValue("@Address", Addressbox.Text);
                            command.Parameters.AddWithValue("@Name", nameBox.Text);
                            command.Parameters.AddWithValue("@Surname", Surnamebox.Text);
                            command.Parameters.AddWithValue("@backupcode", randomNumber);
                            
                            dbconnect.OpenCon();
                            command.CommandText = insertQurey;
                            command.ExecuteNonQuery();
                            MessageBox.Show("Your back code is : " + randomNumber.ToString() + " Dont give it to any one use it when you lost your password ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MessageBox.Show("added Successfully");
                            reste();
                            staff s = new staff();
                            s.loaduser();
                            dbconnect.CloseCon();


                        }
                    }
                    else
                    {
                        MessageBox.Show("Wrong phone number ", "Warning record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("This Username Exisest ", "Warning record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please fill in all required fields before continuing.", "Warning record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
