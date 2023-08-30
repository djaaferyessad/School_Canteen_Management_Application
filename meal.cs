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
    public partial class meal : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        string selectedid;
        public meal()
        {
            InitializeComponent();
            loadmeals();
            AddItemsToComboBox("SELECT * FROM Manage_meal1", type_box);
            AddItemsToComboBox("SELECT * FROM Manage_meal2", room_box);
        }

        private void loadmeals()
        {
            int i = 0;
            mealTable.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM meal_list ", dbconnect.getCon());
            dbconnect.OpenCon();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                mealTable.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            dbconnect.CloseCon();
        }

        private void add_btn_Click(object sender, EventArgs e)
        {
            int id=50000;
            string lastIdQuery = "SELECT TOP 1 Id FROM meal_list ORDER BY id DESC";
            SqlCommand lastIdCommand = new SqlCommand(lastIdQuery, dbconnect.getCon());
            dbconnect.OpenCon();
            object lastIdResult = lastIdCommand.ExecuteScalar();
            dbconnect.CloseCon();

            if (lastIdResult != null && !DBNull.Value.Equals(lastIdResult))
            {
                id = Convert.ToInt32(lastIdResult)+1;
            }
            try { 
            string insertQurey = "INSERT INTO meal_list (id,Name,Type,Day,Hour,Room) VALUES (@id,@Name,@Type,@Day,@Hour,@Room)";
            SqlCommand command = new SqlCommand(insertQurey, dbconnect.getCon());
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@Name", nameinput.Text);
            command.Parameters.AddWithValue("@Type", type_box.Text);
            command.Parameters.AddWithValue("@Day",day_box.Text );
             string text = hour1_box.Text + " " + hour2_box.Text;
            command.Parameters.AddWithValue("@Hour", text);
            command.Parameters.AddWithValue("@Room", room_box.Text);
            dbconnect.OpenCon();
            command.CommandText = insertQurey;
            command.ExecuteNonQuery();
            MessageBox.Show("added Successfully");
            dbconnect.CloseCon();
                loadmeals();
                restboxes();

        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }
}
        private void restboxes()
        {
            nameinput.Text = "";
            type_box.Text = "";
            day_box.Text = "";
            hour1_box.Text = "";
            hour2_box.Text = "";
            room_box.Text = "";
        }

        private void mealTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            nameinput.Text = mealTable.Rows[e.RowIndex].Cells[2].Value.ToString();
            type_box.Text = mealTable.Rows[e.RowIndex].Cells[3].Value.ToString();
            day_box.Text = mealTable.Rows[e.RowIndex].Cells[4].Value.ToString();
            string text = mealTable.Rows[e.RowIndex].Cells[5].Value.ToString();
            string[] parts = text.Split(' ');
            hour1_box.Text = parts[0];
            hour2_box.Text = parts[1];
             room_box.Text = mealTable.Rows[e.RowIndex].Cells[6].Value.ToString();
            selectedid = mealTable.Rows[e.RowIndex].Cells[1].Value.ToString();

        }

        private void update_btn_Click(object sender, EventArgs e)
        {
            try
            {


                if (MessageBox.Show("Are you sure you want to update this user ", "Update Record ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // string updateQurey = "UPDATE Std_list SET Name=@name,Surname=@surname,Sex=@sex,Type=@Type,School_system=@school,GroupNum=@group WHERE id LIKE '" + selectedid + "' ";
                    SqlCommand command = new SqlCommand("UPDATE meal_list SET Name=@name,Type=@Type,Day=@Day,Hour=@Hour,Room=@Room WHERE Id LIKE @selectedid ", dbconnect.getCon());
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Name", nameinput.Text);
                    command.Parameters.AddWithValue("@Type", type_box.Text);
                    command.Parameters.AddWithValue("@Day", day_box.Text);
                    command.Parameters.AddWithValue("@Hour", hour1_box.Text + " " + hour2_box.Text);
                    command.Parameters.AddWithValue("@Room", room_box.Text);
                    command.Parameters.AddWithValue("@selectedid", selectedid);

                    dbconnect.OpenCon();
                    command.ExecuteNonQuery();
                    dbconnect.CloseCon();
                    MessageBox.Show("updated successfully");
                    restboxes();
                    loadmeals();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = mealTable.SelectedRows;
            List<string> primaryids = new List<string>();
            foreach (DataGridViewRow row in selectedRows)
            {
                string primaryidvalue = row.Cells[1].Value.ToString();
                primaryids.Add(primaryidvalue);
            }
            if (primaryids.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to Delete this users", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string deleteStatement = $"DELETE FROM meal_list WHERE {"Id"} IN ({string.Join(",", primaryids)})";
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
            loadmeals();
        }

        private void refresh_btn_Click(object sender, EventArgs e)
        {
            loadmeals();
            search_box.Text = "";
        }

        private void search_btn_Click(object sender, EventArgs e)
        {
            if (name_radio.Checked)
            {
                int i = 0;
                mealTable.Rows.Clear();
                string search = search_box.Text;
                string selectQurey = "SELECT * FROM meal_list WHERE Name = @name  ";
                SqlCommand cm = new SqlCommand(selectQurey, dbconnect.getCon());
                cm.Parameters.AddWithValue("@name", search);
                dbconnect.OpenCon();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    mealTable.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
                }
                dr.Close();
                dbconnect.CloseCon();
            }
            else if (id_radio.Checked)
            {

                int i = 0;
                mealTable.Rows.Clear();
                string search = search_box.Text;
                string selectQurey = "SELECT * FROM meal_list WHERE id = @id  ";
                SqlCommand cm = new SqlCommand(selectQurey, dbconnect.getCon());
                cm.Parameters.AddWithValue("@id", search);
                dbconnect.OpenCon();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    mealTable.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
                }
                dr.Close();
                dbconnect.CloseCon();
            }
        }

        private void filter_btn_Click(object sender, EventArgs e)
        {
            mealfilter m = new mealfilter(type_box, day_box, hour1_box, hour2_box, room_box,ref mealTable);
            m.ShowDialog();
        }
        // add items to combobox :

        public void AddItemsToComboBox(string query, ComboBox comboBox)
        {
            var command = new SqlCommand(query, dbconnect.getCon());
            dbconnect.OpenCon();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    comboBox.Items.Add(reader.GetString(0));
                }
            }
            dbconnect.CloseCon();

        }
    }

}
