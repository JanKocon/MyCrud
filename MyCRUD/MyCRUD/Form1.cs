using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCRUD
{
    public partial class Form1 : Form
    {
        SQLiteConnection sqlite;

        public void InitSQLiteConnection()
        {
            string path = Environment.CurrentDirectory + "\\JohhnyDataBase.db";
            sqlite = new SQLiteConnection("Data source=" + path);
        }
        public DataTable selectQuery(string query)
        {
            SQLiteDataAdapter adapter;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                sqlite.Open();  
                cmd = sqlite.CreateCommand();
                cmd.CommandText = query;
                adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt); 
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(sqlite != null)
                    sqlite.Close();
            }
            return dt;
        }
        public Form1()
        {
            InitializeComponent();
            InitSQLiteConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            DataTable dt = selectQuery("select * from Clients");
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ID_Label.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            Name_TextBox.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            Number_TextBox.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            Address_TextBox.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            
        }

        private void Delete_OnClick(object sender, EventArgs e)
        {
            string query = $"delete from Clients where ID={ID_Label.Text}";
            selectQuery(query);
            UpdateDataGrid();

        }

        private void Update_OnClick(object sender, EventArgs e)
        {
            string number = Number_TextBox.Text.Trim();
            if (number.All(char.IsDigit))
            {
                string query = $"update Clients set Name='{Name_TextBox.Text}'," +
                               $" Number={Number_TextBox.Text}," +
                               $" Address='{Address_TextBox.Text}' " +
                               $"where ID={ID_Label.Text}";
                selectQuery(query);
                UpdateDataGrid();
            }
            else
                MessageBox.Show("Number has to be numerical");

        }

        private void Add_Button_OnClick(object sender, EventArgs e)
        {
            string query = "insert into Clients (Name, Number, Address)" +
                           $" values (('{Name_TextBox.Text}'),('{Number_TextBox.Text}'),('{Address_TextBox.Text}'))";
            selectQuery(query);
            UpdateDataGrid();
        }
    }
}
