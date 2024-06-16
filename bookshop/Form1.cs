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
    public partial class Form1 : Form
    {
        UserApp user = new UserApp();
        DbContent content = new DbContent();
        public Form1()
        {
            InitializeComponent();
            this.FormClosed += Close;
        }

        private void Close(object sender, EventArgs e) => Application.Exit();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(textBox1.Text != "" && textBox2.Text != "")
                {
                    user.Email = textBox1.Text;
                    if (textBox2.Text.Length < 8)
                    {
                        throw new Exception("Слишком легкий пароль");
                    }
                    user.Password = textBox2.Text;
                    content.AddUser(user);
                    MessageBox.Show("Пользователь добавлен");
                    this.Hide();
                    new Form2(user).ShowDialog();
                }
                else
                {
                    throw new Exception("Введите все данные");
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Form3().ShowDialog();
        }
    }
}
