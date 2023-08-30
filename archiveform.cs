using ClosedXML.Excel;
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
    public partial class archiveform : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public archiveform()
        {
            InitializeComponent();
            loaduser();
        }
        public void loaduser()
        {
            int i = 0;
            archiveTable.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM Archive_list ", dbconnect.getCon());
            dbconnect.OpenCon();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                archiveTable.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
            }
            dr.Close();
            dbconnect.CloseCon();
        }

        private void search_btn_Click(object sender, EventArgs e)
        {
            if (name_radio.Checked)
            {
                int i = 0;
                archiveTable.Rows.Clear();
                string search = search_box.Text;
                string selectQurey = "SELECT * FROM Archive_list WHERE Name = @name  ";
                SqlCommand cm = new SqlCommand(selectQurey, dbconnect.getCon());
                cm.Parameters.AddWithValue("@name", search);
                dbconnect.OpenCon();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    archiveTable.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                }
                dr.Close();
                dbconnect.CloseCon();
            }
            else if (surname_radio.Checked)
            {
                int i = 0;
                archiveTable.Rows.Clear();
                string search = search_box.Text;
                string selectQurey = "SELECT * FROM Archive_list WHERE Surname = @surname  ";
                SqlCommand cm = new SqlCommand(selectQurey, dbconnect.getCon());
                cm.Parameters.AddWithValue("@surname", search);
                dbconnect.OpenCon();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    archiveTable.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                }
                dr.Close();
                dbconnect.CloseCon();
            }
            if (id_radio.Checked)
            {

                int i = 0;
                archiveTable.Rows.Clear();
                try
                {
                    int search = Convert.ToInt32(search_box.Text);
                    string selectQurey = "SELECT * FROM Archive_list WHERE id = @id  ";
                    SqlCommand cm = new SqlCommand(selectQurey, dbconnect.getCon());
                    cm.Parameters.AddWithValue("@id", search);
                    dbconnect.OpenCon();
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        i++;
                        archiveTable.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                    }
                    dr.Close();
                    dbconnect.CloseCon();
                }
                catch { MessageBox.Show("Please enter valid format", "error", MessageBoxButtons.OK, MessageBoxIcon.Error); } 
            }
        }

        private void export_btn_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        foreach (DataGridViewColumn col in archiveTable.Columns)
                        {
                            if (col.Index <= 8) // Only include first 9 columns
                            {
                                Type dataType = col.ValueType ?? typeof(string); // Use string as default data type if ValueType is null
                                dt.Columns.Add(col.HeaderText, dataType);
                            }
                        }
                        foreach (DataGridViewRow row in archiveTable.Rows)
                        {
                            DataRow dRow = dt.NewRow();
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.Value != null) // check for null values before adding them
                                {
                                    if (cell.ColumnIndex < 9) // only add the first 9 columns
                                    {
                                        dRow[cell.ColumnIndex] = cell.Value;
                                    }
                                }
                            }
                            dt.Rows.Add(dRow);
                        }

                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            workbook.Worksheets.Add(dt, "Archive_list");
                            workbook.SaveAs(sfd.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = archiveTable.SelectedRows;
            List<int> primaryids = new List<int>();
            foreach (DataGridViewRow row in selectedRows)
            {
                int primaryidvalue = Convert.ToInt32(row.Cells[1].Value);
                primaryids.Add(primaryidvalue);
            }
            if (primaryids.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to Delete this users", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string deleteStatement = $"DELETE FROM Archive_list WHERE {"id"} IN ({string.Join(",", primaryids)})";
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
            loaduser();
        }

        private void refresh_btn_Click(object sender, EventArgs e)
        {
            loaduser();
            search_box.Text = "";
        }

        private void filter_btn_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            filter f = new filter(form.yearinput, form.branchinput, form.groupeinput, form.systeminput, ref archiveTable);
            f.ShowDialog();
        }

        private void archiveTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = archiveTable.Columns[e.ColumnIndex].Name;
            if(colName == "Delete")
            {
                if (MessageBox.Show("are you sure you want  to delete this user ?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbconnect.OpenCon();
                    cm = new SqlCommand("DELETE FROM Archive_list WHERE id = " + archiveTable.Rows[e.RowIndex].Cells[1].Value.ToString() + "", dbconnect.getCon());
                    cm.ExecuteNonQuery();
                    dbconnect.CloseCon();
                    MessageBox.Show("user has been Successfully deleted!");
                    loaduser();
                }
            }
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }
    }
}
