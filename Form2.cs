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

namespace DESKTOP_APP
{
    public partial class Form2 : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm;
        SqlDataReader dr;
        string imageUrl;
        bool addedimg = false;

        public Form2()
        {
            InitializeComponent();
            AddItemsToComboBox("SELECT * FROM Manage_std1", branchinput);
            AddItemsToComboBox("SELECT * FROM Manage_std3", yearinput);
            AddItemsToComboBox("SELECT * FROM Manage_std2", groupeinput);
            AddItemsToComboBox("SELECT * FROM Manage_std4", systeminput);

        }


        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

       private void reste()
        {
            nameinput.Text = "";
            surnameinput.Text = "";
            branchinput.Text = "";
            yearinput.Text = "";
            groupeinput.Text = "";
            systeminput.Text = "";
            pictureBox1.Image = Image.FromFile(Application.StartupPath + @"\icon\image.png");
        }
// adding student 
       private void add_btn_Click_1(object sender, EventArgs e)
        {
            int id = 600000;
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

                string lastIdQuery = "SELECT TOP 1 id FROM Std_list ORDER BY id DESC";
                SqlCommand lastIdCommand = new SqlCommand(lastIdQuery, dbconnect.getCon());
                dbconnect.OpenCon();
                object lastIdResult = lastIdCommand.ExecuteScalar();
                dbconnect.CloseCon();

                if (lastIdResult != null && !DBNull.Value.Equals(lastIdResult))
                {
                    id = (int)lastIdResult + 1;
                }
                Image img = pictureBox1.Image;
               
                string insertQurey = "INSERT INTO Std_list (id,Name,Surname,Sex,Year,Branch,GroupNum,School_system,Image,ImageUrl) VALUES (@id,@Name,@Surname,@Sex,@Year,@Branch,@GroupNum,@School_system,@Image,@ImageUrl)";

                //  VALUES ('" + id + "','" + nameinput.Text + "','" + surnameinput.Text + "','" + selected +
                //  "','" + yearinput.Text + "','" + branchinput.Text + "' ,'" + groupeinput.Text + "','" + systeminput.Text + "','" + convertimagetobyte(img) + "','" + imageUrl + "' )";

                SqlCommand command = new SqlCommand(insertQurey, dbconnect.getCon());
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@Name", nameinput.Text);
                command.Parameters.AddWithValue("@Surname", surnameinput.Text);
                command.Parameters.AddWithValue("@Sex", selected);
                command.Parameters.AddWithValue("@Year", yearinput.Text);
                command.Parameters.AddWithValue("@Branch", branchinput.Text);
                command.Parameters.AddWithValue("@GroupNum", groupeinput.Text);
                command.Parameters.AddWithValue("@School_system", systeminput.Text);
                command.Parameters.AddWithValue("@Image", convertimagetobyte(pictureBox1.Image));
                if (!addedimg)
                {
                    imageUrl = Application.StartupPath + @"\icon\image.png";
                    command.Parameters.AddWithValue("@ImageUrl", imageUrl);   
                }
                else
                {
                    command.Parameters.AddWithValue("@ImageUrl", imageUrl);
                }
                dbconnect.OpenCon();
                command.CommandText = insertQurey;
                command.ExecuteNonQuery();
                MessageBox.Show("added Successfully");
                dbconnect.CloseCon();
                // id =(int) dataGridView1.SelectedRows[0].Cells[0].Value+1;
                reste();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }
        }
        // image to byte 
        byte[] convertimagetobyte(Image imag)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imag.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }



        // importing from excel
        private void importbtn_Click(object sender, EventArgs e)
        {
           
            Exelform excel = new Exelform();
            excel.ShowDialog();

        }

        // adding to the combo boxes

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

        private void addimgbutton_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
          
        }

        private void addbuttonimg_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Image files (*.jpg;*.jpeg)|*.jpg;*.jpeg", Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    imageUrl = ofd.FileName;
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                    addedimg = true;
                }    
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
