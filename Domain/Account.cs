using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public BigInteger Balance { get; set; }
        public BigInteger Limit { get; set; }
    }
}
