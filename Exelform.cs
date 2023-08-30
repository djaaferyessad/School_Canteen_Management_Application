using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Z.Dapper.Plus;

namespace DESKTOP_APP
{
    public partial class Exelform : Form
    {
        public Exelform()
        {
            InitializeComponent();
          
        }
        DataTableCollection tableCollection;
        int countstudents = 0;

        private void pathbtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel 97-2003 Workbook|*.xls|Excel workbook|*.xlsx" })
            {
                if(ofd.ShowDialog() == DialogResult.OK )
                {
                    filenametext.Text = ofd.FileName;
                    using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() {UseHeaderRow = true }
                            });
                            tableCollection = result.Tables;
                            combosheet.Items.Clear();
                            foreach(DataTable table in tableCollection)
                                combosheet.Items.Add(table.TableName);
                        }
                    }
                }
            }
        }

        private void combosheet_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            DataTable dt = tableCollection[combosheet.SelectedItem.ToString()];
            DataGridViewExcel.DataSource = dt;
            if (dt != null)
            {
                List<Student> students = new List<Student>();
                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Student student = new Student();
                        student.id = Convert.ToInt32(dt.Rows[i]["id"]);
                        student.Name = dt.Rows[i]["Name"].ToString();
                        student.Surname = dt.Rows[i]["Surname"].ToString();
                        student.Sex = dt.Rows[i]["Sex"].ToString();
                        student.Year = dt.Rows[i]["Year"].ToString();
                        student.Branch = dt.Rows[i]["Branch"].ToString();
                        student.GroupNum = dt.Rows[i]["GroupNum"].ToString();
                        student.School_system = dt.Rows[i]["School_system"].ToString();


                        students.Add(student);
                        countstudents = i + 1;
                    }
                    studentBindingSource.DataSource = students;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void importbtn_Click(object sender, EventArgs e)
        {
            
            Dbconnect1 dbconnect1 = new Dbconnect1();
            try
            {
              
                DapperPlusManager.Entity<Student>().Table("Std_list");
                List<Student> students = studentBindingSource.DataSource as List<Student>;

                if (students != null)
                {
                    using (IDbConnection db = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\djaaf\OneDrive\Documents\Student_manag.mdf;Integrated Security=True;Connect Timeout=30"))
                    {
                        db.BulkInsert(students);
                    }
                }

                string message = "Students have been added successfully!\n" +
                 "Total number of students added: " + countstudents.ToString();
                MessageBox.Show(message, "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DataGridViewExcel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
