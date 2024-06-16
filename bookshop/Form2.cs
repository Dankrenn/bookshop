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
    public partial class Form2 : Form
    {
        UserApp user;
        DbContent dbContent = new DbContent();
        public List<Book> booksCush = new List<Book>();
        public Form2(UserApp user)
        {
            this.user = user;
            InitializeComponent();
            label1.Text = user.Email;
            this.FormClosed += Close;
            comboBox1.Items.Add("По автору возр.");
            comboBox1.Items.Add("По автору убыв.");
            comboBox1.Items.Add("По цене возр.");
            comboBox1.Items.Add("По цене убыв.");
        }

        private void Close(object sender, EventArgs e) => Application.Exit();

        private void Form2_Load(object sender, EventArgs e)
        {
          List<Book> books =  dbContent.GetBooks();
          Update(books);
        }

        public void Update(List<Book> books)
        {
            flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < books.Count; i++)
            {
                MyUserControl myUserControl = new MyUserControl(user, books[i],this);
                myUserControl.label1.Text = books[i].Name;
                myUserControl.label2.Text = books[i].Autor;
                myUserControl.label3.Text = books[i].TypeBook;
                myUserControl.label4.Text = books[i].Prise.ToString();
                myUserControl.label5.Text = books[i].Count.ToString();
                flowLayoutPanel1.Controls.Add(myUserControl);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            int index = comboBox1.SelectedIndex;
            List<Book> books = dbContent.GetBooksForStrAndIndex(text, index);
            Update(books);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            int index = comboBox1.SelectedIndex;
            List<Book> books =  dbContent.GetBooksForStrAndIndex(text, index);
            Update(books);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Form5(booksCush,this,user).ShowDialog();
        }
    }
}
