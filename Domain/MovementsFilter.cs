using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MovementsFilter
    {
        public string? Id { get; set; }
        public string Account_Id { get; set; }
        public Type? Type { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
