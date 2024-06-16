using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookshop
{
    public class Order
    {
        public int idOrder {  get; set; }
        public int idUser { get; set; }
        public int Prise { get; set; }
        public string Status { get; set; }
    }
}
