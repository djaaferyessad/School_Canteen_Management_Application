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
    public partial class staff : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        int id;
        public staff()
        {
            InitializeComponent();
            loaduser();
        }
        public void loaduser()
        {
            int i = 0;
           userstable.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM login_tabl ", dbconnect.getCon());
            dbconnect.OpenCon();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                userstable.Rows.Add( dr[3].ToString(), dr[6].ToString() + dr[7].ToString(), dr[5].ToString(), dr[4].ToString(), dr[2].ToString(), dr[0].ToString());
            }
            dr.Close();
            dbconnect.CloseCon();
        }
        private void reste()
        {
            nameinput.Text = "";
            surnameinput.Text = "";
            addressinput.Text = "";
            phoneinput.Text = "";
            designation.Text = "";
            usernameinput.Text = "";
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            staffAdd s = new staffAdd();
            s.ShowDialog();
            loaduser();
        }

        private void userstable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            id = Convert.ToInt32(userstable.Rows[e.RowIndex].Cells[0].Value.ToString());
            string namesurname = userstable.Rows[e.RowIndex].Cells[1].Value.ToString();
            string[] words = namesurname.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            nameinput.Text = words[0];
            surnameinput.Text = words[1];
            addressinput.Text = userstable.Rows[e.RowIndex].Cells[2].Value.ToString();
            phoneinput.Text = userstable.Rows[e.RowIndex].Cells[3].Value.ToString();
            designation.Text = userstable.Rows[e.RowIndex].Cells[4].Value.ToString();
            usernameinput.Text = userstable.Rows[e.RowIndex].Cells[5].Value.ToString();


        }

        private void update_btn_Click(object sender, EventArgs e)
        {
            if(nameinput.Text != "" && surnameinput.Text != "" && addressinput.Text != "" && phoneinput.Text != "" && designation.Text != "" && usernameinput.Text != "")
            {
                try
                {
                    if (phoneinput.Text.Length == 10)
                    {
                        int selectid = id;
                        SqlCommand command = new SqlCommand("UPDATE login_tabl SET Username=@Username,Role=@role,phone=@phone,Address=@Address,Name=@Name,Surname=@Surname WHERE id LIKE @selectid ", dbconnect.getCon());
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Username", usernameinput.Text);
                        command.Parameters.AddWithValue("@Role", designation.Text);
                        command.Parameters.AddWithValue("@phone", phoneinput.Text);
                        command.Parameters.AddWithValue("@Address", addressinput.Text);
                        command.Parameters.AddWithValue("@Name", nameinput.Text);
                        command.Parameters.AddWithValue("@Surname", surnameinput.Text);
                        command.Parameters.AddWithValue("@selectid", selectid);

                        dbconnect.OpenCon();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Updated Successfully");
                        dbconnect.CloseCon();
                        loaduser();
                        reste();

                    }
                    else
                    {
                        MessageBox.Show("Wrong phone number ", "Warning record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("This Username Exisest ", "Warning record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please fill in all required fields before continuing.", "Warning record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void search_fonction(string text)
        {
            int i = 0;
            userstable.Rows.Clear();
            string search = search_box.Text;
            string[] words = search.Split(' ');
            string selectQurey;
            if (text == "name")
             selectQurey = "SELECT * FROM login_tabl WHERE Name = @name OR Surname = @name OR (Surname =@word2 AND Name =@word1)";
            else
             selectQurey = "SELECT * FROM login_tabl WHERE Username = @name";
            SqlCommand cm = new SqlCommand(selectQurey, dbconnect.getCon());
            cm.Parameters.AddWithValue("@name", search);
            if (words.Length >= 2)
            {
                    cm.Parameters.AddWithValue("@word1", words[0]);
                    cm.Parameters.AddWithValue("@word2", words[1]);
             }
            dbconnect.OpenCon();
            try
            {
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    userstable.Rows.Add(dr[3].ToString(), dr[6].ToString() + dr[7].ToString(), dr[5].ToString(), dr[4].ToString(), dr[2].ToString(), dr[0].ToString());
                }
                dr.Close();
                dbconnect.CloseCon();
            }
            catch (Exception ex) { MessageBox.Show("there is no one with this " + search_box.Text + "", "error", MessageBoxButtons.OK); }
        }
        private void search_btn_Click(object sender, EventArgs e)
        {
            if (name_radio.Checked)
            {
                search_fonction("name");
            }
            else if (id_radio.Checked)
            {
                search_fonction("Username");
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataGridViewSelectedRowCollection selectedRows = userstable.SelectedRows;
            List<string> primaryids = new List<string>();
            foreach (DataGridViewRow row in selectedRows)
            {
                string primaryidvalue = (row.Cells[5].Value.ToString());
                primaryids.Add(primaryidvalue);
                i++;
            }
            if (primaryids.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to Delete this users", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string deleteStatement = $"DELETE FROM login_tabl WHERE {"Username"} IN ('{string.Join("','", primaryids)}')";
                    SqlCommand command = new SqlCommand(deleteStatement, dbconnect.getCon());
                    dbconnect.OpenCon();
                    command.ExecuteNonQuery();
                    MessageBox.Show(i.ToString() + " rows deleted.");
                }
            }
            else
            {
                MessageBox.Show("No rows selected.");
            }
            loaduser();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (userstable.SelectedRows.Count == 1)
            {
                foreach (DataGridViewRow row in userstable.SelectedRows)
                {
                    staffPass p = new staffPass(row.Cells[5].Value.ToString());
                    p.ShowDialog();
                }

            }
            else
            {
                MessageBox.Show("You have either selected too many rows or no rows at all.Please select the appropriate number of rows to perform this action.", "Warning record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void refresh_btn_Click(object sender, EventArgs e)
        {
            loaduser();
            search_box.Text = "";
        }
    }
    }

