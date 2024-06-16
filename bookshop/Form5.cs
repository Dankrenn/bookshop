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
    public partial class Form5 : Form
    {
       public List<Book> booksCush;
        Form2 form2;
        UserApp user;
        DbContent dbContent =new DbContent();
        List<MyUserControlBook> myUserControlBooks = new List<MyUserControlBook>();
        public Form5(List<Book> booksCush, Form2 form2,UserApp user)
        {
            this.user = user;
            this.booksCush = booksCush;
            this.form2 = form2;
            InitializeComponent();
            this.FormClosed += Close;

        }

        private void Close(object sender, EventArgs e) => Application.Exit();

        private void Form5_Load(object sender, EventArgs e)
        {
            Update(booksCush);
        }
        public void Update(List<Book> books)
        {
            myUserControlBooks.Clear();
            flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < books.Count; i++)
            {
                MyUserControlBook myUserControlBook = new MyUserControlBook(books[i],form2,this);
                myUserControlBook.label1.Text = books[i].Name;
                myUserControlBook.label2.Text = books[i].Count.ToString();
                flowLayoutPanel1.Controls.Add(myUserControlBook);
                myUserControlBooks.Add(myUserControlBook);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            form2.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int prise = 0;
            for (int i = 0; i < booksCush.Count; i++)
            {
                int count = Convert.ToInt32(myUserControlBooks[i].label3.Text);
                Book book = myUserControlBooks[i].book;
                prise += book.Prise * count;
            }
            for (int i = 0; i < booksCush.Count; i++)
            {
                int count = Convert.ToInt32(myUserControlBooks[i].label3.Text);
                Book book = myUserControlBooks[i].book;
                dbContent.AddOrder(book, user, count,prise);
            }
            form2.Update(dbContent.GetBooks());
            this.Hide();
            form2.Show();
        }
    }
}
