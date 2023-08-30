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
   
    public partial class CanteenApp : Form
    {
        bool homecollapse = true;
        bool homecollapse2;
        int selectedid;
        int groupe;
        string username;
 
        Dbconnect1 dbconnect = new Dbconnect1();
        public CanteenApp(string user)
        {
            InitializeComponent();
            openchild(new Form2());
            panelofselectedbutton.BringToFront();
            username = user;
            string typeofuser = "WRITER";
            if (user != "WRITER")
            typeofuser =  Typeofuser();
            if (typeofuser == "WRITER")
            {
               
                studentcontainer.MaximumSize = new Size(188, 70);
                meal_panel.MaximumSize = new Size(188, 75);
                statbtn.Visible = false;
                record_btn.Visible = false;
                record_btn.Visible = true;
                logout.Visible = true;
                staffbtn.Visible = false;
                this.Resize += new EventHandler(Form1_Resize);
            }
            this.Resize += new EventHandler(Form1_Resize);
        }

        private string Typeofuser()
        {
            string selectQuery = "SELECT Role FROM login_tabl WHERE Username = @username";
            SqlCommand cmd = new SqlCommand(selectQuery, dbconnect.getCon());
            cmd.Parameters.AddWithValue("@username", username);
            dbconnect.OpenCon();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read()) // Call Read() to advance to the first row
                {

                    string userType = dr.GetString(0);

                    if (userType == "WRITER    ")
                    {
                        return "WRITER";
                    }
                    else
                    {
                        return "ADMIN";
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return "ADMIN";
            }


        }

        private void gunaComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

      /*  private void gunaTextBox1_TextChanged(object sender, EventArgs e)
        {
            yearinput.DropDownStyle = ComboBoxStyle.Simple;
        } */
        private void getTable()
        {
            string selectQurey = "SELECT * FROM Std_list";
            SqlCommand command = new SqlCommand(selectQurey, dbconnect.getCon());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
           // dataGridView1.DataSource = table;
            // dataGridView1.ReadOnly = true;
        }

        int i = 0;
        private void hometimer_Tick(object sender, EventArgs e)
        {
            
            if(homecollapse)
            {
                i++;
                studentcontainer.Height += 10;
                statbtn.Location = new Point(statbtn.Location.X, statbtn.Location.Y + 10);
                meal_panel.Location = new Point(meal_panel.Location.X, meal_panel.Location.Y + 10);
                record_btn.Location = new Point(record_btn.Location.X, record_btn.Location.Y + 10);
                staffbtn.Location = new Point(staffbtn.Location.X, staffbtn.Location.Y +10);
                if (studentcontainer.Height == studentcontainer.MaximumSize.Height)
                {   
                    homecollapse = false;
                    hometimer.Stop();
                }

            }
            else 
            {
                studentcontainer.Height -= 10;
                statbtn.Location = new Point(statbtn.Location.X, statbtn.Location.Y - 10);
                meal_panel.Location = new Point(meal_panel.Location.X, meal_panel.Location.Y - 10);
                record_btn.Location = new Point(record_btn.Location.X, record_btn.Location.Y - 10);
                staffbtn.Location = new Point(staffbtn.Location.X, staffbtn.Location.Y - 10);
                if (studentcontainer.Height == studentcontainer.MinimumSize.Height)
                {
                    homecollapse = true;
                    hometimer.Stop();
                }

            }
         
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            hometimer.Start();
            managestd_btn.Image = Image.FromFile(Application.StartupPath + @"\icon\student (7).png");
            managestd_btn.ForeColor = ColorTranslator.FromHtml("#A22F2F");
            mealbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\lunch (2).png");
            mealbtn.ForeColor = Color.White;
            record_btn.Image = Image.FromFile(Application.StartupPath + @"\icon\Group 159.png");
            record_btn.ForeColor = Color.White;
            statbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\pie-chart.png");
            statbtn.ForeColor = Color.White;
            staffbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\otherones\management white.png");
            staffbtn.ForeColor = Color.White;
            panelofselectedbutton.Location = new Point(panelofselectedbutton.Location.X, 97);
            if (!homecollapse2)
                hometimer2.Start();
            
        }

        private void mealbtn_Click(object sender, EventArgs e)
        {
            hometimer2.Start();
            mealbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\lunch (1).png");
            mealbtn.ForeColor = ColorTranslator.FromHtml("#A22F2F");
            managestd_btn.Image = Image.FromFile(Application.StartupPath + @"\icon\student (8).png");
            managestd_btn.ForeColor = Color.White;
            record_btn.Image = Image.FromFile(Application.StartupPath + @"\icon\Group 159.png");
            record_btn.ForeColor = Color.White;
            statbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\pie-chart.png");
            statbtn.ForeColor = Color.White;
            staffbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\otherones\management white.png");
            staffbtn.ForeColor = Color.White;
            if (!homecollapse)
                hometimer.Start();

            panelofselectedbutton.Location = new Point(panelofselectedbutton.Location.X, 152);
            panelofselectedbutton.BringToFront();

        }

        private void gunaPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void hometimer2_Tick(object sender, EventArgs e)
        {
            if (homecollapse2)
            {
                meal_panel.Height += 10;
                statbtn.Location = new Point(statbtn.Location.X, statbtn.Location.Y + 10);
                record_btn.Location = new Point(record_btn.Location.X, record_btn.Location.Y + 10);
                staffbtn.Location = new Point(staffbtn.Location.X, staffbtn.Location.Y + 10);
                if (meal_panel.Height == meal_panel.MaximumSize.Height)
                {
                    homecollapse2 = false;
                    hometimer2.Stop();
                }

            }
            else
            {
                meal_panel.Height -= 10;
                statbtn.Location = new Point(statbtn.Location.X, statbtn.Location.Y - 10);
                record_btn.Location = new Point(record_btn.Location.X, record_btn.Location.Y - 10);
                staffbtn.Location = new Point(staffbtn.Location.X, staffbtn.Location.Y - 10);
                if (meal_panel.Height == meal_panel.MinimumSize.Height)
                {
                    homecollapse2 = true;
                    hometimer2.Stop();
                }

            }
        }
        
        private void importbtn_Click(object sender, EventArgs e)
        {
            Exelform exel = new Exelform();
            exel.ShowDialog();
            
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            
        }

        Form activeform = null;
        public void openchild(Form childform)
        {
            if(activeform != null)
            {
                activeform.Close();
            }
            activeform = childform;
           childform.TopLevel = false;
            childform.FormBorderStyle = FormBorderStyle.None;
            childform.Dock=DockStyle.Fill;
            Form1.Controls.Add(childform);
            Form1.Tag = childform;
            childform.BringToFront();
            childform.Show();


        }

       
        public void gunaButton8_Click(object sender, EventArgs e)
        {
            openchild(new Form2());
        }

        private void childformpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void admisionbtn_Click(object sender, EventArgs e)
        {
            openchild(new Form2());
            admision_btn.ForeColor = ColorTranslator.FromHtml("#BEA1A1");
            list_btn.ForeColor = Color.White;
            school_btn.ForeColor = Color.White;
            academic_btn.ForeColor = Color.White;
        }

        private void list_btn_Click(object sender, EventArgs e)
        {
            openchild(new liststudentform());
            list_btn.ForeColor = ColorTranslator.FromHtml("#BEA1A1");
            admision_btn.ForeColor = Color.White;
            school_btn.ForeColor = Color.White;
            academic_btn.ForeColor = Color.White;
        }

        private void school_btn_Click(object sender, EventArgs e)
        {
            openchild(new archiveform());
            school_btn.ForeColor = ColorTranslator.FromHtml("#BEA1A1");
            admision_btn.ForeColor = Color.White;
            list_btn.ForeColor = Color.White;
            academic_btn.ForeColor = Color.White;
        }
        private void gunaButton2_Click(object sender, EventArgs e)
        {
            openchild(new others());
            academic_btn.ForeColor = ColorTranslator.FromHtml("#BEA1A1");
            admision_btn.ForeColor = Color.White;
            list_btn.ForeColor = Color.White;
            school_btn.ForeColor = Color.White;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void gunaButton1_Click_1(object sender, EventArgs e)
        {
            record_btn.Image = Image.FromFile(Application.StartupPath + @"\icon\attendant-list.png");
            record_btn.ForeColor = ColorTranslator.FromHtml("#A22F2F")  ;
            managestd_btn.Image = Image.FromFile(Application.StartupPath + @"\icon\student (8).png");
            managestd_btn.ForeColor = Color.White;
            mealbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\lunch (2).png");
            mealbtn.ForeColor = Color.White;
            statbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\pie-chart.png");
            statbtn.ForeColor = Color.White;
            staffbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\otherones\management white.png");
            staffbtn.ForeColor = Color.White;
            panelofselectedbutton.Location = new Point(panelofselectedbutton.Location.X, 194);
            if (!homecollapse)
            hometimer.Start();
            if (!homecollapse2)
            hometimer2.Start();

            openchild(new attendance());
    

        }
        

        private void panelofselectedbutton_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void add_meal_btn_Click(object sender, EventArgs e)
        {

        }

        private void add_meal_btn_Click_1(object sender, EventArgs e)
        {
            meal m = new meal();
            openchild(m);
            //panelofselectedbutton.Location = new Point(panelofselectedbutton.Location.X, 3);

        }

        private void others_btn_Click(object sender, EventArgs e)
        {
            openchild(new mealmanage());
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();    
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void statbtn_Click(object sender, EventArgs e)
        {
            statbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\pie-chart (1).png");
            statbtn.ForeColor = ColorTranslator.FromHtml("#A22F2F");
            managestd_btn.Image = Image.FromFile(Application.StartupPath + @"\icon\student (8).png");
            managestd_btn.ForeColor = Color.White;
            mealbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\lunch (2).png");
            mealbtn.ForeColor = Color.White;
            panelofselectedbutton.Location = new Point(panelofselectedbutton.Location.X, 237);
            record_btn.Image = Image.FromFile(Application.StartupPath + @"\icon\Group 159.png");
            record_btn.ForeColor = Color.White;
            staffbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\otherones\management white.png");
            staffbtn.ForeColor = Color.White;
            if (!homecollapse)
                hometimer.Start();
            if (!homecollapse2)
                hometimer2.Start();

            openchild(new statsform());
        }

        private void staffbtn_Click(object sender, EventArgs e)
        {
            staffbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\otherones\management red-1.png");
            staffbtn.ForeColor = ColorTranslator.FromHtml("#A22F2F");
            statbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\pie-chart.png");
            statbtn.ForeColor = Color.White;
            managestd_btn.Image = Image.FromFile(Application.StartupPath + @"\icon\student (8).png");
            managestd_btn.ForeColor = Color.White;
            mealbtn.Image = Image.FromFile(Application.StartupPath + @"\icon\lunch (2).png");
            mealbtn.ForeColor = Color.White;
            panelofselectedbutton.Location = new Point(panelofselectedbutton.Location.X, 282);
            record_btn.Image = Image.FromFile(Application.StartupPath + @"\icon\Group 159.png");
            record_btn.ForeColor = Color.White;
            if (!homecollapse)
                hometimer.Start();
            if (!homecollapse2)
                hometimer2.Start();


            openchild(new staff());
        }

        private void logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            login l = new login();
            l.ShowDialog();
            
            
        }

        private void CanteenApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }
    }
}
