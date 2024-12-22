using CSharpEgitimKampi501.Dtos;
using Dapper;
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

namespace CSharpEgitimKampi501
{
    public partial class Product : Form
    {
        public Product()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection("Server=DESKTOP-UDCIU0S\\SQLEXPRESS; Initial Catalog=EgitimKampi501Db; Integrated Security=True"); 
        private async void btnList_Click(object sender, EventArgs e)
        {
            string query = "Select * From Tbl_Product";
            var values = await connection.QueryAsync<ResultProductDto>(query);
            dataGridView1.DataSource = values;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "Insert Into Tbl_Product (ProductName,ProductStock,ProductPrice,ProductCategory) values (@productName, @productStock, @productPrice, @productCategory)";
            var parameter = new DynamicParameters();
            parameter.Add("@productName", txtProductName.Text);
            parameter.Add("@productStock", txtProductStock.Text);
            parameter.Add("@productPrice", txtProductPrice.Text);
            parameter.Add("@productCategory", txtCategory.Text);
            await connection.ExecuteAsync(query, parameter);
            MessageBox.Show("Yeni Ürün Ekleme İşlemi Başarılı.");

        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "Delete From Tbl_Product Where ProductId=@productId";
            var parameter = new DynamicParameters();
            parameter.Add("@productId", txtProductId.Text);
            await connection.ExecuteAsync(query, parameter);
            MessageBox.Show("Silme işlemi başarılı.");
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            string query = "Update Tbl_Product Set ProductName=@productName,ProductStock=@productStock,ProductPrice=@productPrice,ProductCategory=@productCategory Where ProductId=@productId";
            var parameter = new DynamicParameters();
            parameter.Add("@productId", txtProductId.Text);
            parameter.Add("@productName", txtProductName.Text);
            parameter.Add("@productStock", txtProductStock.Text);
            parameter.Add("@productPrice", txtProductPrice.Text);
            parameter.Add("@productCategory", txtCategory.Text);
            await connection.ExecuteAsync(query, parameter);
            MessageBox.Show("Ürün Güncelleme İşlemi Başarılı.", "Güncelleme", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private async void Product_Load(object sender, EventArgs e)
        {
            string query1 = "Select Count(*) From Tbl_Product";
            var totalCount = await connection.QueryFirstOrDefaultAsync<int>(query1);
            lblTotalProductCount.Text = totalCount.ToString();

            string query2 = "Select ProductName From Tbl_Product Where ProductPrice=(Select Max(ProductPrice) From Tbl_Product)";
            var maxPrice = await connection.QueryFirstOrDefaultAsync<string>(query2);
            lblMaxPrice.Text = maxPrice;

            string query3 = "Select Count(Distinct(ProductCategory)) From Tbl_Product";
            var countCategory = await connection.QueryFirstOrDefaultAsync<int>(query3);
            lblCategoryCount.Text = countCategory.ToString();

        }
    }
}
