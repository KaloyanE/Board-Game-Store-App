using System;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.tableTableAdapter.Fill(this.database1DataSet.Board_games);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Datasetview form2 = new Datasetview();
            form2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Searching form3 = new Searching();
            form3.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cart form4 = new Cart();
            form4.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
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


namespace WindowsFormsApp3
{
    public partial class Datasetview : Form
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Kokito\\source\\repos\\WindowsFormsApp3\\WindowsFormsApp3\\Database1.mdf;Integrated Security=True;Connect Timeout=30";

        public Datasetview()
        {
            InitializeComponent();
            PopulateDataGridView();
        }
        private void RefreshDataGridView()
        {            
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Board_games";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                try
                {                    
                    connection.Open();
                    adapter.Fill(dt);
                    tableDataGridView.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error refreshing DataGridView: " + ex.Message);
                }
            }
        }
        private void PopulateDataGridView()
        {          
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {               
                string query = "SELECT * FROM Board_games";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);   
                adapter.Fill(dataTable);
            }
            tableDataGridView.DataSource = dataTable;
        }
         
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string type = textBox2.Text;
            int age = Convert.ToInt32(textBox3.Text);
            string playtime = textBox4.Text;
            double price = Convert.ToDouble(textBox5.Text);
            int quantity = Convert.ToInt32(textBox6.Text);

            string query = "INSERT INTO Board_games (Name, Type, [Recomended age], [Average playtime], Price, Quantity) VALUES (@Name, @Type, @Age, @Playtime, @Price, @Quantity)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Type", type);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Playtime", playtime);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@Quantity", quantity);
                try
                {                   
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show(rowsAffected + " row(s) inserted successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting data: " + ex.Message);
                }

            }
            PopulateDataGridView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nameToDelete = textBox1.Text;
            string query = "DELETE FROM Board_games WHERE Name = @Name";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {              
                command.Parameters.AddWithValue("@Name", nameToDelete);
                try
                {                 
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                   
                    if (rowsAffected > 0)
                        MessageBox.Show("Game deleted successfully!");
                    else
                        MessageBox.Show("No game found with the specified name.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting game: " + ex.Message);
                }
            }
            RefreshDataGridView();
            }

        private void button3_Click(object sender, EventArgs e)
        {
            string IDToEdit = textBox7.Text;
          
            string query = "SELECT * FROM Board_games WHERE Id = @Id";
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", IDToEdit);
                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        textBox1.Text = reader["Name"].ToString();
                        textBox2.Text = reader["Type"].ToString();
                        textBox3.Text = reader["Recomended age"].ToString();
                        textBox4.Text = reader["Average playtime"].ToString();
                        textBox5.Text = reader["Price"].ToString();
                        textBox6.Text = reader["Quantity"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No game found with the specified name.");
                    }
                   
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving game details: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int itemId = Convert.ToInt32(textBox7.Text);
           
            string nameToUpdate = textBox1.Text;
            string type = textBox2.Text;
            int recommendedAge = Convert.ToInt32(textBox3.Text);
            int averagePlaytime = Convert.ToInt32(textBox4.Text);
            double price = Convert.ToDouble(textBox5.Text);
            int quantity = Convert.ToInt32(textBox6.Text);


            string query = "UPDATE Board_games SET Name = @Name, Type = @Type, [Recomended age] = @RecommendedAge, [Average playtime] = @AveragePlaytime, Price = @Price, Quantity = @Quantity WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                
                command.Parameters.AddWithValue("@Id", itemId);
                command.Parameters.AddWithValue("@Name", nameToUpdate);
                command.Parameters.AddWithValue("@Type", type);
                command.Parameters.AddWithValue("@RecommendedAge", recommendedAge);
                command.Parameters.AddWithValue("@AveragePlaytime", averagePlaytime);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@Quantity", quantity);

                try
                {                  
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Game details updated successfully!");
                        RefreshDataGridView();
                    }
                    else
                        MessageBox.Show("No game found with the specified ID.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating game details: " + ex.Message);
                }
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                textBox7.Focus();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                textBox1.Focus();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                textBox2.Focus();
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
            {
                e.Handled = true;
                textBox5.Focus();
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > 0)
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                textBox3.Focus();
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                textBox4.Focus();
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                textBox6.Focus();
            }
        }

 
    }
}
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


namespace WindowsFormsApp3
{
    public partial class Searching : Form
    {
        private string connectionString= "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Kokito\\source\\repos\\WindowsFormsApp3\\WindowsFormsApp3\\Database1.mdf;Integrated Security=True;Connect Timeout=30";
        public Searching()
        {
            InitializeComponent();

            listViewResults.View = View.Details;
            listViewResults.Columns.Add("ID", 100);
            listViewResults.Columns.Add("Name", 150);
            listViewResults.Columns.Add("Type", 150);
            listViewResults.Columns.Add("Recommended Age", 100);
            listViewResults.Columns.Add("Average Playtime", 120);
            listViewResults.Columns.Add("Price", 80);
            listViewResults.Columns.Add("Quantity", 80);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int searchId;
            if (!int.TryParse(textBox1.Text, out searchId))
            {
                MessageBox.Show("Please enter a valid item ID.");
                return;
            }

            string query = "SELECT * FROM Board_games WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", searchId);

                try
                {
                    
                    connection.Open();

                   
                    SqlDataReader reader = command.ExecuteReader();

                    
                    listViewResults.Items.Clear();

                    
                    if (reader.Read())
                    {     
                        ListViewItem item = new ListViewItem(reader["Id"].ToString());
                        item.SubItems.Add(reader["Name"].ToString());
                        item.SubItems.Add(reader["Type"].ToString());
                        item.SubItems.Add(reader["Recomended age"].ToString());
                        item.SubItems.Add(reader["Average playtime"].ToString());
                        item.SubItems.Add(reader["Price"].ToString());
                        item.SubItems.Add(reader["Quantity"].ToString());

                        listViewResults.Items.Add(item);
                    }
                    else
                    {
                        MessageBox.Show("No item found with the specified ID.");
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching for item: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            string searchName = textBox2.Text.Trim();        
            string query = "SELECT * FROM Board_games WHERE Name LIKE @Name";
          
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {              
                command.Parameters.AddWithValue("@Name", "%" + searchName + "%");

                try
                {                  
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    listViewResults.Items.Clear();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["Id"].ToString());
                        item.SubItems.Add(reader["Name"].ToString());
                        item.SubItems.Add(reader["Type"].ToString());
                        item.SubItems.Add(reader["Recomended age"].ToString());
                        item.SubItems.Add(reader["Average playtime"].ToString());
                        item.SubItems.Add(reader["Price"].ToString());
                        item.SubItems.Add(reader["Quantity"].ToString());

                        listViewResults.Items.Add(item);
                    }

                    reader.Close();

                    if (listViewResults.Items.Count == 0)
                    {
                        MessageBox.Show("No items found with the specified name.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching for items: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string searchType = textBox3.Text.Trim();          
            string query = "SELECT * FROM Board_games WHERE Type LIKE @Type";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Type", "%" + searchType + "%");

                try
                {
                   
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    listViewResults.Items.Clear();
                    while (reader.Read())
                    {                       
                        ListViewItem item = new ListViewItem(reader["Id"].ToString());
                        item.SubItems.Add(reader["Name"].ToString());
                        item.SubItems.Add(reader["Type"].ToString());
                        item.SubItems.Add(reader["Recomended age"].ToString());
                        item.SubItems.Add(reader["Average playtime"].ToString());
                        item.SubItems.Add(reader["Price"].ToString());
                        item.SubItems.Add(reader["Quantity"].ToString());

                        listViewResults.Items.Add(item);
                    }
                    reader.Close();

                    if (listViewResults.Items.Count == 0)
                    {
                        MessageBox.Show("No items found with the specified type.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching for items: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int searchAge;
            if (!int.TryParse(textBox4.Text.Trim(), out searchAge))
            {
                MessageBox.Show("Please enter a valid age.");
                return;
            }
            string query = "SELECT * FROM Board_games WHERE [Recomended age] >= @Age";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Age", searchAge);

                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    listViewResults.Items.Clear();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["Id"].ToString());
                        item.SubItems.Add(reader["Name"].ToString());
                        item.SubItems.Add(reader["Type"].ToString());
                        item.SubItems.Add(reader["Recomended age"].ToString());
                        item.SubItems.Add(reader["Average playtime"].ToString());
                        item.SubItems.Add(reader["Price"].ToString());
                        item.SubItems.Add(reader["Quantity"].ToString());

                        listViewResults.Items.Add(item);
                    }
                    reader.Close();

                    if (listViewResults.Items.Count == 0)
                    {
                        MessageBox.Show("No items found with the specified age.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching for items: " + ex.Message);
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                textBox1.Focus();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                textBox3.Focus();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                textBox2.Focus();
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                textBox4.Focus();
            }
        }
    }

        
    }
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp3
{
    public partial class Cart : Form
    {
        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Kokito\\source\\repos\\WindowsFormsApp3\\WindowsFormsApp3\\Database1.mdf;Integrated Security=True;Connect Timeout=30";
        private double totalPrice = 0;
        
        public Cart()
        {
            InitializeComponent();

            listViewCart.View = View.Details;
            listViewCart.Columns.Add("Id", 100);
            listViewCart.Columns.Add("Name", 150);
            listViewCart.Columns.Add("Type", 100);
            listViewCart.Columns.Add("Price", 100);
            listViewCart.Columns.Add("Quantity in Cart", 100);
            listViewCart.Columns.Add("Available Quantity", 100);
        }
        private double CalculateTotalPrice()
        {
            double total = 0;
            foreach (ListViewItem item in listViewCart.Items)
            {
                double price = Convert.ToDouble(item.SubItems[3].Text);
                int quantity = int.Parse(item.SubItems[4].Text);
                total += price * quantity;
            }
            return total;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int itemId;
            if (!int.TryParse(textBox2.Text, out itemId))
            {
                MessageBox.Show("Please enter a valid item ID.");
                return;
            }
            bool itemInCart = false;
            foreach (ListViewItem item in listViewCart.Items)
            {
                if (item.SubItems[0].Text == itemId.ToString())
                {
                    int quantityInCart = int.Parse(item.SubItems[4].Text);
                    quantityInCart++;
                    item.SubItems[4].Text = quantityInCart.ToString();

                    itemInCart = true;
                    break;
                }
            }

            if (!itemInCart)
            {              
                string query = "SELECT * FROM Board_games WHERE Id = @Id";
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", itemId);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {                           
                            ListViewItem item = new ListViewItem(reader["Id"].ToString());
                            item.SubItems.Add(reader["Name"].ToString());
                            item.SubItems.Add(reader["Type"].ToString());
                            item.SubItems.Add(reader["Price"].ToString());
                            item.SubItems.Add("1");
                            item.SubItems.Add(reader["Quantity"].ToString());
                            listViewCart.Items.Add(item);

                            totalPrice = CalculateTotalPrice();
                            textBox1.Text = totalPrice.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Item not found.");
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error adding item: " + ex.Message);
                    }
                }
            }
            else
            {
                totalPrice = CalculateTotalPrice();
                textBox1.Text = totalPrice.ToString();
            }
            }

        private void button2_Click(object sender, EventArgs e)
        {
            int itemId;
            if (!int.TryParse(textBox2.Text, out itemId))
            {
                MessageBox.Show("Please enter a valid item ID.");
                return;
            }

            ListViewItem itemToRemove = null;
            foreach (ListViewItem item in listViewCart.Items)
            {
                if (item.SubItems[0].Text == itemId.ToString())
                {
                    itemToRemove = item;
                    break;
                }
            }

            if (itemToRemove != null)
            {
                // Check if the item has multiple quantities in the cart
                int quantityInCart = int.Parse(itemToRemove.SubItems[4].Text);
                if (quantityInCart > 1)
                {                  
                    quantityInCart--;
                    itemToRemove.SubItems[4].Text = quantityInCart.ToString();
                }
                else
                {                   
                    listViewCart.Items.Remove(itemToRemove);
                }

                
                totalPrice = CalculateTotalPrice();
                textBox1.Text = totalPrice.ToString();

                MessageBox.Show("Item removed from the cart.");
            }
            else
            {
                MessageBox.Show("Item not found in the cart.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listViewCart.Items.Clear();            
            totalPrice = 0;
            textBox1.Text = totalPrice.ToString();
            MessageBox.Show("Cart cleared.");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewCart.Items)
            {
                int itemId = int.Parse(item.SubItems[0].Text);
                int quantityInCart = int.Parse(item.SubItems[4].Text);
                int availableQuantity = int.Parse(item.SubItems[5].Text);

                if (quantityInCart > availableQuantity)
                {
                    MessageBox.Show($"Insufficient stock for item with ID {itemId}. Please reduce the quantity in the cart.");
                    return;
                }
                else if (quantityInCart == availableQuantity)
                {
                    MessageBox.Show($"The last item with ID {itemId} has been sold.");
                }

                UpdateQuantityInDatabase(itemId, quantityInCart);
            }

            listViewCart.Items.Clear();

            totalPrice = 0;
            textBox1.Text = totalPrice.ToString();

            MessageBox.Show("Cart checked out.");
        }
        private void UpdateQuantityInDatabase(int itemId, int quantityToSubtract)
        {            
            string query = "UPDATE Board_games SET Quantity = Quantity - @QuantityToSubtract WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", itemId);
                command.Parameters.AddWithValue("@QuantityToSubtract", quantityToSubtract);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating quantity in database: " + ex.Message);
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                textBox1.Focus();
            }
        }
    }
}
