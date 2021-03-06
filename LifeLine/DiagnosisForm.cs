using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace LifeLine
{
    public partial class DiagnosisForm : Form
    {
        public DiagnosisForm()
        {
            InitializeComponent();
        }
        static string constring = ConfigurationManager.ConnectionStrings["LifeLine.Properties.Settings.LifeLine"].ConnectionString;
        SqlConnection Con = new SqlConnection(constring);
        void populatecombo()
        {
            string sql = "select * from Patient";
            SqlCommand cmd = new SqlCommand(sql, Con);
            SqlDataReader rdr;
            try
            {
                Con.Open();
                DataTable dt = new DataTable();
                dt.Columns.Add("PatId", typeof(int));
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                PatientIdCb.ValueMember = "PatId";
                PatientIdCb.DataSource = dt;
                Con.Close();
            }
            catch
            { 
            }
        }
        string patname;
        void fecthpatientname()
        {
            Con.Open();
            //string query = ("select * from Patient where PatId=" + PatientIdCb.SelectedValue.ToString()+"");
            SqlCommand cmd = new SqlCommand("select * from Patient ", Con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                patname = dr["PatName"].ToString();
                PatientTb.Text = patname;
            }
            Con.Close();
        }
        void populate()
        {
            if (Con.State == ConnectionState.Closed) {
                Con.Open();
            }
            string query = "select * from Diagnosis";
            SqlDataAdapter da = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            DiagnosisGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Home h = new Home();
            h.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DiagId.Text == "" || MedicineTb.Text == "" || DiagnosisTb.Text == "" || PatientTb.Text == "" || MedicineTb.Text == "" )
                MessageBox.Show("No Empty Fill Accepted");
            else
            {
                Con.Open();
             string query = "insert into Diagnosis(PatId,PatName,Symptoms,Diagnosis,Medicines)" +
                    " values(" + PatientIdCb.SelectedValue.ToString() + ",'" + PatientTb.Text + "','" + SymptomsTb.Text + "','" + DiagnosisTb.Text + "','" +MedicineTb.Text + "')";
             SqlCommand cmd = new SqlCommand(query, Con);
             cmd.ExecuteNonQuery();
                MessageBox.Show("Diagnosis Successfully Added");
                Con.Close();
                populate();
            }
        }

        private void DiagnosisForm_Load(object sender, EventArgs e)
        {
            populatecombo();
            populate();
            DiagId.Visible = false;
            button3.Enabled = false;
            button2.Enabled = false;
        }

        private void PatientIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            fecthpatientname();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DiagId.Text == "")
                MessageBox.Show("Enter The Diagnosis Id");
            else
            {
                Con.Open();
                string query = "delete from Diagnosis where DiagId=" + DiagId.Text + "";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Diagnosis Successfully Deleted");
                Con.Close();
                populate();
            }
        }

        private void DiagnosisGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DiagId.Text = DiagnosisGV.SelectedRows[0].Cells[0].Value.ToString();
            PatientIdCb.SelectedValue = DiagnosisGV.SelectedRows[0].Cells[1].Value.ToString();
            PatientTb.Text = DiagnosisGV.SelectedRows[0].Cells[2].Value.ToString();
            SymptomsTb.Text = DiagnosisGV.SelectedRows[0].Cells[3].Value.ToString();
            DiagnosisTb.Text = DiagnosisGV.SelectedRows[0].Cells[4].Value.ToString();
            MedicineTb.Text = DiagnosisGV.SelectedRows[0].Cells[5].Value.ToString();
            PatientNamelbl.Text = DiagnosisGV.SelectedRows[0].Cells[2].Value.ToString();
            Diagnosislbl.Text = DiagnosisGV.SelectedRows[0].Cells[4].Value.ToString();
            Symptomslbl.Text = DiagnosisGV.SelectedRows[0].Cells[3].Value.ToString();
            medicineslbl.Text = DiagnosisGV.SelectedRows[0].Cells[5].Value.ToString();
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Con.Open();
            string query = "update Diagnosis set PatId = "+PatientIdCb.SelectedValue.ToString()+",PatName ='" + PatientTb.Text + "',Symptoms='" + SymptomsTb.Text + "',Diagnosis='"+DiagnosisTb.Text+"',Medicines='"+MedicineTb.Text+"' where DiagId=" + DiagId.Text + "";
            SqlCommand cmd = new SqlCommand(query, Con);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Diagnosis Successfully updated");
            Con.Close();
            populate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(printPreviewDialog1.ShowDialog()==DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
           
        }

        private void printDocument1_PrintPage_1(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(label4.Text , new Font("Century Gothic", 25, FontStyle.Bold), Brushes.Red, new Point(230));
            e.Graphics.DrawString(PatientNamelbl.Text+"\n"+Diagnosislbl.Text +"\n"+Symptomslbl.Text +"\n"+medicineslbl.Text, new Font("Century Gothic", 12, FontStyle.Regular), Brushes.Black, new Point(130,150));
           // e.Graphics.DrawString(label10.Text + "\n\t"+label11.Text, new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Red, new Point(230,380));
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PatientIdCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
