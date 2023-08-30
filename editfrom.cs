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
    public partial class editfrom : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        internal string id_text_box;

        public editfrom(ComboBox combobox1, ComboBox combobox2, ComboBox combobox3, ComboBox combobox4)
        {
            InitializeComponent();
            year_box.Items.AddRange(combobox1.Items.Cast<string>().ToArray());
            branch_box.Items.AddRange(combobox2.Items.Cast<string>().ToArray());
            group_box.Items.AddRange(combobox3.Items.Cast<string>().ToArray());
            school_box.Items.AddRange(combobox4.Items.Cast<string>().ToArray());    
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void update_btn_Click(object sender, EventArgs e)
        {
            string selected;
            if (maleradio.Checked)
            {
                selected = maleradio.Text;
            }
            else
            {
                selected = femaleradio.Text;
            }
            try
            {


                if (MessageBox.Show("Are you sure you want to update this user ", "Update Record ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int selectedid = Convert.ToInt32(idtext_box.Text);
                    // string updateQurey = "UPDATE Std_list SET Name=@name,Surname=@surname,Sex=@sex,Year=@year,School_system=@school,GroupNum=@group WHERE id LIKE '" + selectedid + "' ";
                    SqlCommand command = new SqlCommand("UPDATE Std_list SET Name=@name,Surname=@surname,Sex=@sex,Year=@year,School_system=@school,GroupNum=@group,Branch=@branch WHERE id LIKE @selectedid ", dbconnect.getCon());
                    command.Parameters.AddWithValue("@name", name_box.Text);
                    command.Parameters.AddWithValue("@surname", surname_box.Text);
                    command.Parameters.AddWithValue("@sex", selected);
                    command.Parameters.AddWithValue("@year", year_box.Text);
                    command.Parameters.AddWithValue("@school", school_box.Text);
                    command.Parameters.AddWithValue("@group", group_box.Text);
                    command.Parameters.AddWithValue("@branch", branch_box.Text);
                    command.Parameters.AddWithValue("@selectedid", selectedid);

                    dbconnect.OpenCon();
                    command.ExecuteNonQuery();
                    dbconnect.CloseCon();
                    MessageBox.Show("updated");
                    this.Close();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void id_box_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
