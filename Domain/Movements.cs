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
        public string Id { get; private set; }
        public string Account_Id { get; private set; }
        public string Description { get; private set; }
        public decimal Amount { get; private set; }
        public Type Type { get; private set; }
        public DateTime Date { get; private set; }

        public Movements() { }

        public Movements(string id, string account_id, string description, decimal amount, Type type)
        {
            Id = id;
            Account_Id = account_id;
            Description = description;
            Amount = amount;
            Type = type;
            Date = DateTime.UtcNow;
        }

        public static Movements CreateDebit(string account_id, decimal amount)
            => new Movements(
                Guid.NewGuid().ToString(),
                account_id,
                $"Debit of value {amount}",
                amount,
                Type.DEBIT);
        public static Movements CreateCredit(string account_id, decimal amount)
            => new Movements(
                Guid.NewGuid().ToString(),
                account_id,
                $"Credit of value {amount}",
                amount,
                Type.CREDIT);
    }
}
