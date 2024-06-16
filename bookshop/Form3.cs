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
    public partial class Form3 : Form
    {
        UserApp user;
        DbContent content = new DbContent();
        public Form3()
        {
            InitializeComponent();
            this.FormClosed += Close;
        }

        private void Close(object sender, EventArgs e) => Application.Exit();

        private void label2_Click(object sender, EventArgs e)
        {
           this.Hide();
            new Form1().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    user = content.GetUserForEmailAndPass(textBox1.Text,textBox2.Text) ;
                    MessageBox.Show("Пользователь авторизован");
                    this.Hide();
                    if(user.Rule == "Админ")
                    {
                        new Form4(user).ShowDialog();
                    }
                    else
                    {
                        new Form2(user).ShowDialog();
                    }
                   
                }
                else
                {
                    throw new Exception("Введите все данные");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
    }
}
