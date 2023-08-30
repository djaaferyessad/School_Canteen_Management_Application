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
    public partial class others : Form
    {
        
        string savename;
        string savename2;
        string savename3;
        string savename4;
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public others()
        {
            InitializeComponent();
            getTable(1, table1);
            getTable(2, table2);  
            getTable(3, table3);
            getTable(4, table4);
        }
        // rest

        private void reste()
        {
            Name_box.Text = "";
            Name_box_2.Text = "";
            name_box3.Text = "";
            name_box4.Text = "";

        }

        // selecting cells from grid view
        private void branchs_table_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Name_box.Text = table1.Rows[e.RowIndex].Cells[1].Value.ToString();    
            savename = Name_box.Text;
            
        }
        private void table2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Name_box_2.Text = table2.Rows[e.RowIndex].Cells[1].Value.ToString();
            savename2 = Name_box_2.Text;
        }

        private void table3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            name_box3.Text = table3.Rows[e.RowIndex].Cells[1].Value.ToString();
            savename3 = name_box3.Text;
        }

        private void table4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            name_box4.Text = table4.Rows[e.RowIndex].Cells[1].Value.ToString();
            savename4 = name_box4.Text;
        }


        // getting tables  
        private void getTable(int e , DataGridView t)
        {
           
            int i = 0;
            t.Rows.Clear();
            string selectname = "SELECT* FROM ";
            cm = new SqlCommand(selectname + "Manage_std"+e.ToString(), dbconnect.getCon());
            dbconnect.OpenCon();
            dr = cm.ExecuteReader();
            while (dr.Read())
            { 
                i++;
                if(e != 4)
                t.Rows.Add(i, dr[0].ToString());
                else
                t.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            dbconnect.CloseCon();
        }

        public void addqurey(int e , string text , string text2)
        {
            if (text == "")
            {
                MessageBox.Show("Please enter valid text");
            }
            else
            {
                try
                {
                    string inserttext1 = "";
                    string inserttext2 = "";
                    string insertQurey = "";
                    if (e != 4)
                    {
                        inserttext1 = "INSERT INTO ";
                        inserttext2 = " VALUES('" + text + "')";
                        insertQurey = inserttext1 + "Manage_std" + e.ToString() + inserttext2;
                    }

                    else
                    {
                        insertQurey = "INSERT INTO Manage_std4 VALUES ('" + text + "','" + text2 + "') ";
                    }

                    SqlCommand command = new SqlCommand(insertQurey, dbconnect.getCon());
                    dbconnect.OpenCon();
                    command.CommandText = insertQurey;
                    command.ExecuteNonQuery();
                    MessageBox.Show("added successufully ");
                    dbconnect.CloseCon();
                    reste();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        public void editqurey(int e,String text,string text2)
        {
            if (text == "" && e != 4 )
            {
                MessageBox.Show("Please enter valid text");
            }
            else
            {
                try
                {
                    string save;
                    string updatequreytext2 = "";
                    save = "";
                    if (e == 1)
                        save = savename;
                    else if (e == 2)
                        save = savename2;
                    else if (e == 3)
                        save = savename3;
                    string updatequreytext1 = "UPDATE ";
                    updatequreytext2 = " SET Name = @name WHERE Name LIKE '" + save + "' ";
                    if (e == 4)
                    {
                        save = savename4;
                        updatequreytext2 = " SET Name = @name, Meal_entitlment = @meal WHERE Name LIKE '" + save + "' ";
                    }
                    string insertQurey = updatequreytext1 + "Manage_std" + e.ToString() + updatequreytext2;
                    SqlCommand command = new SqlCommand(insertQurey, dbconnect.getCon());
                    dbconnect.OpenCon();
                    command.Parameters.AddWithValue("@name", text);
                    if (e == 4)
                        command.Parameters.AddWithValue("@meal", text2);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Updated successufully ");
                    dbconnect.CloseCon();
                    reste();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        private void deletequrey(int e , DataGridView t)
        {
            DataGridViewSelectedRowCollection selectedRows = t.SelectedRows;
            List<string> names = new List<string>();
            foreach (DataGridViewRow row in selectedRows)
            {
                string namevalue = row.Cells[1].Value.ToString();
                names.Add(namevalue);
            }
            if (names.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string deleteStatement = "DELETE FROM Manage_std" + e.ToString() + " WHERE Name IN ('" + string.Join("','", names) + "')";
                    SqlCommand command = new SqlCommand(deleteStatement, dbconnect.getCon());
                    dbconnect.OpenCon();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected.ToString() + " rows deleted.");
                    getTable(1, table1);
                }
            }
            else
            {
                MessageBox.Show("No rows selected.");
            }
        }

        // Manage branch
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (Name_box.Text != "")
            {
                addqurey(1,Name_box.Text,"");
                getTable(1,table1);

            }
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            deletequrey(1, table1);
            getTable(1, table1  );
        }

        private void edit_btn_Click(object sender, EventArgs e)
        {
            editqurey(1, Name_box.Text,"");
            getTable(1, table1);
        }

        // Manage groups 

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            addqurey(2,Name_box_2.Text,"");
            getTable(2, table2);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            editqurey(2,Name_box_2.Text,"");
            getTable(2, table2);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            deletequrey(2, table2);
            getTable(2, table2);
        }

        // years section 
        private void guna2Button7_Click(object sender, EventArgs e)
        {
            addqurey(3, name_box3.Text,"");
            getTable(3, table3);

        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            editqurey(3, name_box3.Text,"");
            getTable(3, table3);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            deletequrey(3, table3);
            getTable(3, table3);
        }

        private void maleradio_CheckedChanged(object sender, EventArgs e)
        {
        }
        // system collection
        private void guna2Button10_Click(object sender, EventArgs e)
        {
                if (maleradio.Checked)
                    addqurey(4, name_box4.Text, maleradio.Text);
                else
                    addqurey(4, name_box4.Text, femaleradio.Text);

                getTable(4, table4);
            
        }
        private void guna2Button11_Click(object sender, EventArgs e)
        {
            if (maleradio.Checked)
                editqurey(4, name_box4.Text, maleradio.Text);
            else
                editqurey(4, name_box4.Text, femaleradio.Text);

            getTable(4, table4);
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            deletequrey(4, table4);
            getTable(4, table4);    
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }

}

