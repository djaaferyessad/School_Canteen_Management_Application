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
    public partial class StudentCard : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm = new SqlCommand();
        int id;
        public StudentCard(string n , string s , string b , string sy ,string i,string g,string l)
        {
            InitializeComponent();
            namesurnametext.Text = n + " " + s;
            branchtext.Text = branchtext.Text + " " + b;
            systemtext.Text = systemtext.Text + " " + sy;
            grouptext.Text = grouptext.Text + " " + g;
            levellabel.Text = levellabel.Text + " " + l;
            id = Convert.ToInt32(i);
            namesurnametext.AutoSize = true;
            int x = (panel1.Width - namesurnametext.Width) / 2;
            int y = (panel1.Height - namesurnametext.Height) / 2;
            namesurnametext.Location = new Point(x, y);
            panel1.Controls.Add(namesurnametext);
            BarcodeWriter writer = new BarcodeWriter() { Format = BarcodeFormat.CODE_128 };

            // Generate the barcode image
            Bitmap barcodeImage = writer.Write(i);

            // Resize the image to fit the PictureBox
            Image resizedImage = new Bitmap(barcodeImage, pictureBox1.Width, pictureBox1.Height);

            // Set the PictureBox properties
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = resizedImage;

        }
        Bitmap bmp;

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            bmp = new Bitmap(418, 281, g);
            Graphics mg = Graphics.FromImage(bmp);
            mg.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, this.Size);
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StudentCard_Load(object sender, EventArgs e)
        {
            // Retrieve image data from database
            SqlCommand cmd = new SqlCommand("SELECT Image FROM Std_list WHERE id = @id", dbconnect.getCon());
            cmd.Parameters.AddWithValue("id", id);
            dbconnect.OpenCon();
            try
            {
                byte[] imageData = (byte[])cmd.ExecuteScalar();
                dbconnect.CloseCon();
                if (imageData != null)
                {
                    guna2PictureBox1.Image = bytetoimage(imageData);

                }
                else
                {
                    guna2PictureBox1.Image = Image.FromFile(@"C:\Users\djaaf\Downloads\icons app_desk\man (1).png");
                }
                // 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Use the retrieved image URL as needed
            // ...
        }
        private Image bytetoimage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return Image.FromStream(ms);
            }
        }
    }
        
    
}
