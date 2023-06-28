using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Account
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal Limit { get; set; }

        public Account(string id, string name, decimal balance, decimal limit)
        {
            this.Id = id;
            this.Name = name;
            this.Balance = balance;
            this.Limit = limit;
        }

        public static Account CreateBasicAccount(string name) 
            => new Account(
                string.Concat(new Random().Next(999999).ToString()), 
                name, 
                1000M,
                1000M);

        public static Account UpdateBalanceWithTransaction(Account account, decimal requestAmount)
        {
            var balanceAvailableAfterCredit = account.Balance - requestAmount;
            return balanceAvailableAfterCredit < 0 ? new Account(account.Id, account.Name, 0, account.Limit - Math.Abs(balanceAvailableAfterCredit)) : new Account(account.Id, account.Name, balanceAvailableAfterCredit, account.Limit);
        }
    }
}
