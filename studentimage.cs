using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_APP
{
    public partial class studentimage : Form
    {
        Dbconnect1 dbconnect = new Dbconnect1();
        SqlCommand cm = new SqlCommand();
        int id;
        public studentimage(string d)
        {
            InitializeComponent();
            id = Convert.ToInt32(d);
        }

        private void studentimage_Load(object sender, EventArgs e)
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
                    pictureBox1.Image = bytetoimage(imageData);

                }
                // 
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            // Retrieve image URL from database
            SqlCommand cmd2 = new SqlCommand("SELECT ImageUrl FROM Std_list WHERE id = @id", dbconnect.getCon());
            cmd2.Parameters.AddWithValue("id", id);
            dbconnect.OpenCon();
            object ptext = cmd2.ExecuteScalar();
            dbconnect.CloseCon();
            if (ptext != null)
            {
                pathtext.Text = ptext.ToString();
            }

            // Use the retrieved image URL as needed
            // ...
        }
        private Image bytetoimage(byte[] data)
        {
            using(MemoryStream ms = new MemoryStream(data))
            {
                return Image.FromStream(ms);
            }
        }

    }
}

