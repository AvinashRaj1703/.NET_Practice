using System;
using System.Data;
using System.Data.SqlClient;

namespace practical8
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private readonly string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              Initial Catalog=INVENTORY_DB;
              Integrated Security=True;
              Connect Timeout=30;
              Encrypt=False";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        // Display records directly from the database
        private void LoadProducts()
        {
            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                string query = @"SELECT PROD_ID, PROD_NAME,
                                        COST_PRICE, SELLING_PRICE, QTY
                                 FROM INVENTORY_RECORD
                                 ORDER BY PROD_ID";

                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }

        // Clear all input fields
        private void ClearFields()
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox5.Text = "";
        }

        // Validate input values
        private bool GetProductDetails(
            out int id,
            out string name,
            out decimal cost,
            out decimal selling,
            out int qty)
        {
            id = 0;
            name = TextBox2.Text.Trim();
            cost = 0;
            selling = 0;
            qty = 0;

            bool valid =
                int.TryParse(TextBox1.Text.Trim(), out id) &&
                decimal.TryParse(TextBox3.Text.Trim(), out cost) &&
                decimal.TryParse(TextBox4.Text.Trim(), out selling) &&
                int.TryParse(TextBox5.Text.Trim(), out qty) &&
                name.Length > 0 &&
                name.Length <= 100 &&
                cost >= 0 &&
                selling >= 0 &&
                qty >= 0;

            if (!valid)
            {
                lblMessage.Text =
                    "Enter valid product details. Prices and quantity cannot be negative.";

                return false;
            }

            return true;
        }

        // Add parameters for INSERT and UPDATE
        private void AddProductParameters(
            SqlCommand cmd,
            int id,
            string name,
            decimal cost,
            decimal selling,
            int qty)
        {
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            cmd.Parameters.Add(
                "@name", SqlDbType.NVarChar, 100).Value = name;

            SqlParameter costParameter =
                cmd.Parameters.Add("@cost", SqlDbType.Decimal);

            costParameter.Precision = 10;
            costParameter.Scale = 2;
            costParameter.Value = cost;

            SqlParameter sellingParameter =
                cmd.Parameters.Add("@selling", SqlDbType.Decimal);

            sellingParameter.Precision = 10;
            sellingParameter.Scale = 2;
            sellingParameter.Value = selling;

            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = qty;
        }

        // INSERT
        protected void Button1_Click(object sender, EventArgs e)
        {
            int id, qty;
            decimal cost, selling;
            string name;

            if (!GetProductDetails(
                out id, out name, out cost, out selling, out qty))
            {
                return;
            }

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO INVENTORY_RECORD
                        (PROD_ID, PROD_NAME, COST_PRICE, SELLING_PRICE, QTY)
                        VALUES (@id, @name, @cost, @selling, @qty)";

                    using (SqlCommand cmd =
                        new SqlCommand(query, con))
                    {
                        AddProductParameters(
                            cmd, id, name, cost, selling, qty);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                lblMessage.Text = "Product inserted successfully.";
                ClearFields();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    lblMessage.Text =
                        "A product with this ID already exists.";
                }
                else
                {
                    lblMessage.Text =
                        "Unable to insert the product. Check the database.";
                }
            }

            LoadProducts();
        }

        // UPDATE
        protected void Button2_Click(object sender, EventArgs e)
        {
            int id, qty;
            decimal cost, selling;
            string name;

            if (!GetProductDetails(
                out id, out name, out cost, out selling, out qty))
            {
                return;
            }

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    string query = @"UPDATE INVENTORY_RECORD
                                     SET PROD_NAME = @name,
                                         COST_PRICE = @cost,
                                         SELLING_PRICE = @selling,
                                         QTY = @qty
                                     WHERE PROD_ID = @id";

                    using (SqlCommand cmd =
                        new SqlCommand(query, con))
                    {
                        AddProductParameters(
                            cmd, id, name, cost, selling, qty);

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            lblMessage.Text =
                                "Product updated successfully.";

                            ClearFields();
                        }
                        else
                        {
                            lblMessage.Text = "Product ID not found.";
                        }
                    }
                }
            }
            catch (SqlException)
            {
                lblMessage.Text =
                    "Unable to update the product. Check the database.";
            }

            LoadProducts();
        }

        // DELETE
        protected void Button3_Click(object sender, EventArgs e)
        {
            int id;

            if (!int.TryParse(TextBox1.Text.Trim(), out id))
            {
                lblMessage.Text = "Enter a valid product ID.";
                return;
            }

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    string query =
                        "DELETE FROM INVENTORY_RECORD WHERE PROD_ID = @id";

                    using (SqlCommand cmd =
                        new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add(
                            "@id", SqlDbType.Int).Value = id;

                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            lblMessage.Text =
                                "Product deleted successfully.";

                            ClearFields();
                        }
                        else
                        {
                            lblMessage.Text = "Product ID not found.";
                        }
                    }
                }
            }
            catch (SqlException)
            {
                lblMessage.Text =
                    "Unable to delete the product. Check the database.";
            }

            LoadProducts();
        }
    }
}