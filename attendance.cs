using DocumentFormat.OpenXml.Office2010.Excel;
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
using ZXing;

namespace DESKTOP_APP
{
    public partial class attendance : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        string entitlment;
        bool right = false ;
        public attendance()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyPress += new KeyPressEventHandler(submitbtn_KeyPress);
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dddd, MMMM dd, yyyy");
            currenttime.Text = formattedDate;
            AddItemsToComboBox("SELECT * FROM Manage_meal1", mealcombo) ;
        }

        private void getinfo2(int id)
        {
            bool exicte = false;
            cm = new SqlCommand("SELECT * FROM Std_list WHERE id = @id ", dbconnect.getCon());
            dbconnect.OpenCon();
            cm.Parameters.AddWithValue("@id", id);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                exicte = true;
                right = true;
                nametextbox.Text = dr.GetString(1);
                surnametextbox.Text = dr.GetString(2);
                sextextbox.Text = dr.GetString(3);
                yeartextbox.Text = dr.GetString(4);
                branchtextbox.Text = dr.GetString(5);
                grouptextbox.Text = dr.GetString(6);
                schooltextbox.Text = dr.GetString(7);
                entitlment = schooltextbox.Text;
                byte[] imageData = dr["Image"] as byte[];
                if (imageData != null)
                {
                    picturebox1.Image = bytetoimage(imageData);
                }
                else
                {
                    picturebox1.Image = Image.FromFile(@"C:\Users\djaaf\Downloads\icons app_desk\man (1).png");
                }

                BarcodeWriter writer = new BarcodeWriter() { Format = BarcodeFormat.CODE_39 };
                Bitmap barcodeImage = writer.Write(id.ToString());
                Image resizedImage = new Bitmap(barcodeImage, picturebox2.Width, picturebox2.Height);
                picturebox2.SizeMode = PictureBoxSizeMode.StretchImage;
                picturebox2.Image = resizedImage;
            }
            if (!exicte)
            {
                right = false;
                MessageBox.Show("This id doesn't exict ", "Warning Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dr.Close();
            dbconnect.CloseCon();
        }
        private void rightofeating(string ent ,int id)
        {
            cm = new SqlCommand("SELECT Meal_entitlment FROM Manage_std4 WHERE Name = @ent ", dbconnect.getCon());
            dbconnect.OpenCon();
            cm.Parameters.AddWithValue("@ent", ent);
            string resault = (string)cm.ExecuteScalar();

            if (resault == "Yes       ")
            {
                panelofattendance.BorderColor = System.Drawing.Color.Green;
                addtothetable(id);
            }
            else
            {
                panelofattendance.BorderColor = System.Drawing.Color.Red;
                MessageBox.Show("this user doesn't have the right to eat","Warning Record",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                idtextbox.Text = "";
            }

        }

        private Image bytetoimage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return Image.FromStream(ms);
            }
        }
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        // add to the table for save his present 
        private void addtothetable(int ID )
        {
            try
            {
              
                    string type = mealcombo.SelectedItem.ToString();
                    string insertQurey = "INSERT INTO " + type + " (id,date,Branch,Level,School_system) VALUES (@id,@date,@Branch,@Level,@School_system)";
                    SqlCommand command = new SqlCommand(insertQurey, dbconnect.getCon());
                    string date = DateTime.Now.ToString("MM/dd/yyyy");
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", ID);
                    command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@Branch", branchtextbox.Text);
                command.Parameters.AddWithValue("@Level", yeartextbox.Text);
                command.Parameters.AddWithValue("@School_system", schooltextbox.Text);

                command.ExecuteNonQuery();
                    MessageBox.Show("The student has been marked present Successfully");
                idtextbox.Text = "";


            }
            catch(Exception ex) 
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show("This Student is already marked","Warning record",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                idtextbox.Text = "";
            }

        }


        private void submitbtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (mealcombo.SelectedIndex != -1)
                {
                    getinfo2(Convert.ToInt32(idtextbox.Text));
                    if (right)
                    {
                        rightofeating(entitlment, Convert.ToInt32(idtextbox.Text));
                    }
                }
                else { MessageBox.Show("Select the type of meal first"); }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void idtextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        // if ok button is been pressed 
        private void submitbtn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    if (mealcombo.SelectedIndex != -1)
                    {
                        getinfo2(Convert.ToInt32(idtextbox.Text));
                        if (right)
                        {
                            rightofeating(entitlment, Convert.ToInt32(idtextbox.Text));
                        }
                    }
                    else { MessageBox.Show("Select the type of meal first"); }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        // add itmes to the meal type combo box 
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
