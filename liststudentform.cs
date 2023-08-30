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
    public partial class liststudentform : Form
    {
        int id;
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        // for the card :
        public static string idcard ;
        public static string systemcard;
        public  static string namecard ;
        public static string surnamecard;
        public static string branchcard ;
        public static string groupcard, levelcard ;

        public liststudentform()
        {
            InitializeComponent();
            loaduser();
        }
        public void loaduser()
        {
            int i = 0;
            studentuser.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM Std_list ", dbconnect.getCon());
            dbconnect.OpenCon();
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                studentuser.Rows.Add(i,dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
            }
            dr.Close();
            dbconnect.CloseCon();
        }



        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void liststudentform_Load(object sender, EventArgs e)
        {

        }

        private void guna2RadioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void guna2RadioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void guna2RadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void datagridviewstudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
       /* private void getTable()
        {
            string selectQurey = "SELECT * FROM Std_list";
            SqlCommand command = new SqlCommand(selectQurey, dbconnect.getCon());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
           // datagrid.DataSource = table;
            //datagrid.ReadOnly = true;
        }
        */
        private void search_btn_Click(object sender, EventArgs e)
        {
            if(name_radio.Checked)
            {
                int i = 0;
                studentuser.Rows.Clear();
                string search = search_box.Text;
                string selectQurey = "SELECT * FROM Std_list WHERE Name = @name  ";
                SqlCommand cm = new SqlCommand(selectQurey, dbconnect.getCon());
                cm.Parameters.AddWithValue("@name", search);
                dbconnect.OpenCon();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    studentuser.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                }
                dr.Close();
                dbconnect.CloseCon();
            }
            else if (surname_radio.Checked)
            {
                int i = 0;
                studentuser.Rows.Clear();
                string search = search_box.Text;
                string selectQurey = "SELECT * FROM Std_list WHERE Surname = @surname  ";
                SqlCommand cm = new SqlCommand(selectQurey, dbconnect.getCon());
                cm.Parameters.AddWithValue("@surname", search);
                dbconnect.OpenCon();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    studentuser.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                }
                dr.Close();
                dbconnect.CloseCon();
            }
            if (id_radio.Checked)
            {
                
                int i = 0;
                studentuser.Rows.Clear();
                try
                {
                    int search = Convert.ToInt32(search_box.Text);
                    string selectQurey = "SELECT * FROM Std_list WHERE id = @id  ";
                    SqlCommand cm = new SqlCommand(selectQurey, dbconnect.getCon());
                    cm.Parameters.AddWithValue("@id", search);
                    dbconnect.OpenCon();
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        i++;
                        studentuser.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                    }
                    dr.Close();
                    dbconnect.CloseCon();
                }
                catch {
                    MessageBox.Show("Please enter valid format ", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      }
            }
        }


        private void grouperadio_Click(object sender, EventArgs e)
        {

        }

        private void gunaElipsePanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        

        private void studentuser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
  
            string colName = studentuser.Columns[e.ColumnIndex].Name;
            if(colName == "Edit" )
            {
                Form2 form = new Form2();
                editfrom edit = new editfrom(form.yearinput,form.branchinput,form.groupeinput,form.systeminput);
                  edit.name_box.Text = studentuser.Rows[e.RowIndex].Cells[2].Value.ToString();
                 edit.surname_box.Text = studentuser.Rows[e.RowIndex].Cells[3].Value.ToString();
                 edit.year_box.Text = studentuser.Rows[e.RowIndex].Cells[5].Value.ToString();
                 edit.branch_box.Text = studentuser.Rows[e.RowIndex].Cells[6].Value.ToString();
                 edit.group_box.Text = studentuser.Rows[e.RowIndex].Cells[7].Value.ToString();
                 edit.school_box.Text = studentuser.Rows[e.RowIndex].Cells[8].Value.ToString();
                 edit.idtext_box.Text = studentuser.Rows[e.RowIndex].Cells[1].Value.ToString();
                edit.ShowDialog();
                loaduser();

               
            }
            if(colName == "Delete")
            {
                if(MessageBox.Show("are you sure you want  to delete this user ?","Delete Record",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    dbconnect.OpenCon();
                    cm = new SqlCommand("DELETE FROM Std_list WHERE id = "+ studentuser.Rows[e.RowIndex].Cells[1].Value.ToString()+"",dbconnect.getCon());
                    cm.ExecuteNonQuery();
                    dbconnect.CloseCon();
                    MessageBox.Show("user has been Successfully deleted!");
                    loaduser();
                }
            }
            if(colName == "image" )
            {
                string ID ;
                ID = studentuser.Rows[e.RowIndex].Cells[1].Value.ToString();
                studentimage s = new studentimage(ID);
                s.ShowDialog();

            }
            if(colName == "archive")
            {
              
                    int id = Convert.ToInt32(studentuser.Rows[e.RowIndex].Cells[1].Value.ToString());
                    string name = studentuser.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string surname = studentuser.Rows[e.RowIndex].Cells[3].Value.ToString();
                    string sex = studentuser.Rows[e.RowIndex].Cells[4].Value.ToString();
                    string year = studentuser.Rows[e.RowIndex].Cells[5].Value.ToString();
                    string branch = studentuser.Rows[e.RowIndex].Cells[6].Value.ToString();
                    string groupNum = studentuser.Rows[e.RowIndex].Cells[7].Value.ToString();
                    string schoolSystem = studentuser.Rows[e.RowIndex].Cells[8].Value.ToString();
                try
                {
                    string insertStatement = $"INSERT INTO Archive_list (id, Name, Surname, Sex, Year, Branch, GroupNum, School_system) VALUES ({id}, '{name}', '{surname}', '{sex}', '{year}', '{branch}', '{groupNum}', '{schoolSystem}')";
                    SqlCommand command = new SqlCommand(insertStatement, dbconnect.getCon());
                    dbconnect.OpenCon();
                    command.ExecuteNonQuery();
                    MessageBox.Show(" student archived.");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
      
            }
           
        }

        private void filter_btn_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            filter f = new filter(form.yearinput, form.branchinput, form.groupeinput, form.systeminput, ref studentuser);
            f.ShowDialog();
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
           
         DataGridViewSelectedRowCollection selectedRows = studentuser.SelectedRows;
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
                    string deleteStatement = $"DELETE FROM Std_list WHERE {"id"} IN ({string.Join(",", primaryids)})";
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
        Form activeform = null;
        private void add_btn_Click(object sender, EventArgs e)
        {
            openchild(new Form2());
            
        }
        public void openchild(Form childform)
        {
            if (activeform != null)
            {
                activeform.Close();
            }
            activeform = childform;
            childform.TopLevel = false;
            childform.FormBorderStyle = FormBorderStyle.None;
            childform.Dock = DockStyle.Fill;
            childformpanel.Controls.Add(childform);
            childformpanel.Tag = childform;
            childform.BringToFront();
            childform.Show();


        }

        private void refresh_btn_Click(object sender, EventArgs e)
        {
            loaduser();
            search_box.Text = "";
        }

        private void card_btn_Click(object sender, EventArgs e)
        {

            if (studentuser.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in studentuser.SelectedRows)
                {
                    namecard = row.Cells[2].Value.ToString();
                    surnamecard = row.Cells[3].Value.ToString();
                    branchcard = row.Cells[6].Value.ToString();
                    groupcard = row.Cells[7].Value.ToString();
                    systemcard = row.Cells[8].Value.ToString();
                    idcard = row.Cells[1].Value.ToString();
                    levelcard = row.Cells[5].Value.ToString();
                }
                StudentCard s = new StudentCard(namecard,surnamecard,branchcard,systemcard,idcard,groupcard,levelcard);
                s.ShowDialog();
            }
        }

        private void archive_btn_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        foreach (DataGridViewColumn col in studentuser.Columns)
                        {
                            if (col.Index <= 8) // Only include first 9 columns
                            {
                                Type dataType = col.ValueType ?? typeof(string); // Use string as default data type if ValueType is null
                                dt.Columns.Add(col.HeaderText, dataType);
                            }
                        }
                        foreach (DataGridViewRow row in studentuser.Rows)
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
                            workbook.Worksheets.Add(dt, "Std_list");
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

        private void archiveBtn_Click(object sender, EventArgs e)
        {
            int i = 0;
                DataGridViewSelectedRowCollection selectedRows = studentuser.SelectedRows;
                foreach (DataGridViewRow row in selectedRows)
                {
                    int id = Convert.ToInt32(row.Cells[1].Value.ToString());
                    string name = row.Cells[2].Value.ToString();
                    string surname = row.Cells[3].Value.ToString();
                    string sex = row.Cells[4].Value.ToString();
                    string year = row.Cells[5].Value.ToString();
                    string branch = row.Cells[6].Value.ToString();
                    string groupNum = row.Cells[7].Value.ToString();
                    string schoolSystem = row.Cells[8].Value.ToString();

                    
                string insertStatement = $"INSERT INTO Archive_list (id, Name, Surname, Sex, Year, Branch, GroupNum, School_system) VALUES ({id}, '{name}', '{surname}', '{sex}', '{year}', '{branch}', '{groupNum}', '{schoolSystem}')";
                try
                {
                    SqlCommand command = new SqlCommand(insertStatement, dbconnect.getCon());
                    dbconnect.OpenCon();
                    command.ExecuteNonQuery();
                    i++;

                } catch (Exception ex) { MessageBox.Show("This id " +id+" already exists " , "warning" , MessageBoxButtons.OK , MessageBoxIcon.Error ); }   
                 
                }
                 MessageBox.Show(i.ToString(), " rows archived.");

        }
        
    }   
}
