using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bookshop
{
    public partial class Form4 : Form
    {
        UserApp user;
        DbContent content = new DbContent();
        public Form4(UserApp user)
        {
            this.user = user;
            InitializeComponent();
            this.FormClosed += Close;
        }

        private void Close(object sender, EventArgs e) => Application.Exit();

        private void Form4_Load(object sender, EventArgs e)
        {
           List<Order> orders = content.GetOrdersForAdmin();
            Updates(orders);
        }

        public void Updates(List<Order> orders)
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (Order order in orders)
            {
                AdminControl adminControl = new AdminControl(order,this);
                adminControl.label1.Text = order.idOrder.ToString();
                adminControl.label2.Text = order.idUser.ToString();
                adminControl.label3.Text = order.Prise.ToString();
                string[] strings = new string[] { "Новый", "Отказан", "Принят" };
                adminControl.comboBox1.Items.AddRange(strings);
                adminControl.comboBox1.SelectedItem = order.Status;
                flowLayoutPanel1.Controls.Add(adminControl);
            }
        }
    }
}
