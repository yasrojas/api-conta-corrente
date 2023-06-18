using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Movements
    {
        public string Id { get; set; }
        public string Account_Id { get; set; }
        public string Description { get; set; }
        public BigInteger Amount { get; set; }
        public Type Type { get;set; }
        public DateTime Date { get; set; }
    }
}
