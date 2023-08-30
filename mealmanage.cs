using Guna.UI2.WinForms;
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
    public partial class mealmanage : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        string savename , savename2;

        public mealmanage()
        {
            InitializeComponent();
            getTable(1, table1);
            getTable(2, table2);
            
        }

        private void reste()
        {
            Name_box.Text = "";
            Name_box_2.Text = "";
        }
        private void getTable(int e, DataGridView t)
        {

            int i = 0;
            t.Rows.Clear();
            string selectname = "SELECT* FROM ";
            cm = new SqlCommand(selectname + "Manage_meal" + e.ToString(), dbconnect.getCon());
            dbconnect.OpenCon();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                if (e != 4)
                    t.Rows.Add(i, dr[0].ToString());
                else
                    t.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            dbconnect.CloseCon();
        }

        private void table2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Name_box_2.Text = table2.Rows[e.RowIndex].Cells[1].Value.ToString();
            savename2 = Name_box_2.Text;
        }

        private void table1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Name_box.Text = table1.Rows[e.RowIndex].Cells[1].Value.ToString();
            savename = Name_box.Text;
        }
        public void addqurey(int e, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("The text box is empty");
            }
            try
            {
                string inserttext1 = "";
                string inserttext2 = "";
                string insertQurey = "";
                    inserttext1 = "INSERT INTO ";
                    inserttext2 = " VALUES('" + text + "')";
                    insertQurey = inserttext1 + "Manage_meal" + e.ToString() + inserttext2;
               


                SqlCommand command = new SqlCommand(insertQurey, dbconnect.getCon());
                dbconnect.OpenCon();
                command.CommandText = insertQurey;
                command.ExecuteNonQuery();
                MessageBox.Show("has been added successufully ");
                dbconnect.CloseCon();
                reste();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void editqurey(int e, String text, string text2)
        {
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("The text box is empty");
            }
            try
                {
                    string save;
                    string updatequreytext2 = "";
                    save = "";
                    if (e == 1)
                        save = savename;
                    else if (e == 2)
                        save = savename2;
                    string updatequreytext1 = "UPDATE ";
                    updatequreytext2 = " SET Name = @name WHERE Name LIKE '" + save + "' ";
                    string insertQurey = updatequreytext1 + "Manage_meal" + e.ToString() + updatequreytext2;
                    SqlCommand command = new SqlCommand(insertQurey, dbconnect.getCon());
                    dbconnect.OpenCon();
                    command.Parameters.AddWithValue("@name", text);
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
        // create the table
        private void creattable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return;
            }

            try
            {
                // create the table with columns for date, id, and type
                string createTableQuery = $"CREATE TABLE {tableName} (id INT, date DATE, Branch NVARCHAR(20), Level NVARCHAR(20), School_system NVARCHAR(20), PRIMARY KEY(id, date))";
                SqlCommand command = new SqlCommand(createTableQuery, dbconnect.getCon());
                dbconnect.OpenCon();
                command.ExecuteNonQuery();
                dbconnect.CloseCon();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message );   
            }

        }
        // delete the table 
        private void deletetable(string tableName)
        {
            
                if (tableName != "") // make sure the user entered a table name
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand($"DROP TABLE {tableName}", dbconnect.getCon()); // create the SQL command to drop the table
                        dbconnect.OpenCon();
                        cmd.ExecuteNonQuery(); // execute the command
                        dbconnect.CloseCon();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting table {tableName}: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a table name!");
                }
            
        }
        // update the name of the table

        private void updatetable(string tableName)
        {
            
                // Check if the user entered a valid table name
                if (!string.IsNullOrWhiteSpace(tableName))
                {
                    // Rename the table
                    try
                    {
                        string oldTableName = savename; // Replace with the name of the table you want to rename
                        string renameStatement = $"EXEC sp_rename '{oldTableName}', '{tableName}'";
                        SqlCommand command = new SqlCommand(renameStatement, dbconnect.getCon());
                        dbconnect.OpenCon();
                        command.ExecuteNonQuery();
                        MessageBox.Show($"Table '{oldTableName}' renamed to '{tableName}'");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error renaming table: {ex.Message}");
                    }
                    finally
                    {
                        dbconnect.CloseCon();
                    }
                }
            
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            creattable(Name_box.Text);
            addqurey(1, Name_box.Text);
            getTable(1, table1);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            
            addqurey(2, Name_box_2.Text);
            getTable(2, table2);
        }

        private void edit_btn_Click(object sender, EventArgs e)
        {
            updatetable(Name_box.Text);
            editqurey(1, Name_box.Text, "");
            getTable(1, table1);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            editqurey(2, Name_box_2.Text, "");
            getTable(2, table2);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            deletequrey(1, table1);
            getTable(1,table1);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            deletequrey(2, table2);
            getTable(2, table2);
        }

        private void deletequrey(int e, DataGridView t)
        {
            DataGridViewSelectedRowCollection selectedRows = t.SelectedRows;
            List<string> names = new List<string>();
            foreach (DataGridViewRow row in selectedRows)
            {
                string namevalue = row.Cells[1].Value.ToString();
                deletetable(namevalue);
                names.Add(namevalue);
            }
            if (names.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string deleteStatement = "DELETE FROM Manage_meal" + e.ToString() + " WHERE Name IN ('" + string.Join("','", names) + "')";
                    SqlCommand command = new SqlCommand(deleteStatement, dbconnect.getCon());
                    dbconnect.OpenCon();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected.ToString() + " rows deleted.");
                }
            }
            else
            {
                MessageBox.Show("No rows selected.");
            }
        }
        



    }
}
