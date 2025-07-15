using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\evasi\Desktop\UNI\Current Assignments\ST\WindowsFormsApp1\Database1.mdf;Integrated Security=True";
        SqlConnection sqlconnection;
        SqlCommand sqlcommand;
        string Query;
        DataTable datatable;
        SqlDataAdapter sqladapter;
        int ID = 0;

        private void DisplayData()
        {
            sqlconnection = new SqlConnection(cs);
            Query = "Select * From Test";
            sqlcommand = new SqlCommand(Query, sqlconnection);
            sqladapter = new SqlDataAdapter();
            datatable = new DataTable();
            sqladapter.SelectCommand = sqlcommand;
            sqladapter.Fill(datatable);
            dataGridView1.DataSource = datatable;
        }

        private void ClearData()
        {
            txt_name.Text = " ";
            txt_state.Text = " ";
            ID = 0;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Test' table. You can move, or remove it, as needed.
            this.testTableAdapter.Fill(this.database1DataSet.Test);

        }

        private void insert_btn_Click(object sender, EventArgs e)
        {
            if (txt_name.Text != " " && txt_state.Text != " ")
            {
                sqlconnection = new SqlConnection(cs);
                sqlconnection.Open();
                Query = "Insert INTO Test(name,state) " + "VALUES(@name,@state)";
                sqlcommand = new SqlCommand(Query,sqlconnection);
                sqlcommand.Parameters.AddWithValue("@name", txt_name.Text);
                sqlcommand.Parameters.AddWithValue("@state", txt_state.Text);
                sqlcommand.ExecuteNonQuery();
                sqlconnection.Close();
                MessageBox.Show("Insert successfully!");
                DisplayData();
                ClearData();
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            txt_name.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txt_state.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

        }

        private void update_btn_Click(object sender, EventArgs e)
        {
            if (txt_name.Text != " " && txt_state.Text != " ")
            {
                sqlconnection = new SqlConnection(cs);
                sqlconnection.Open();
                Query = "Update Test Set name=@name, state = @state Where Id = @id";
                sqlcommand = new SqlCommand(Query, sqlconnection);
                sqlcommand.Parameters.AddWithValue("@id", ID);
                sqlcommand.Parameters.AddWithValue("@name", txt_name.Text);
                sqlcommand.Parameters.AddWithValue("@state", txt_state.Text);
                sqlcommand.ExecuteNonQuery();
                sqlconnection.Close();
                MessageBox.Show("Record updated successfully!");
                DisplayData();
                ClearData();
            }
        }
    }
}
