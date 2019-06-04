using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskApi.Models
{
    public class User
    {
        public long? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public string Patronymic { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthDate { get; set; }

        public decimal Balance { get; private set; }

        public User()
        {
            Balance = 0;
        }

        public void AddToBalance(decimal sum)
        {
            Balance += sum;
        }

        public void Withdraw(decimal sum)
        {
            if (sum > Balance)
            {
                throw new ArgumentException("Insufficient amount of money on the balance");
            }
            else
            {
                Balance -= sum;
            }
        }
    }
}
