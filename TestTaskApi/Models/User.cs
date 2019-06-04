using System;
using System.ComponentModel.DataAnnotations;

namespace TestTaskApi.Models
{
    /// <summary>
    /// Class for the User entity
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id of user
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Name of user
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Surname
        /// </summary>
        [Required]
        public string Surname { get; set; }

        /// <summary>
        /// Patronymic
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Birthdate
        /// </summary>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Balance
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Constructor for setting 
        /// balance to 0
        /// </summary>
        public User()
        {
            Balance = 0;
        }

        /// <summary>
        /// A method to add money to 
        /// the balance
        /// </summary>
        /// <param name="sum">Sum to add</param>
        public void AddToBalance(decimal sum)
        {
            Balance += sum;
        }

        /// <summary>
        /// A method to withdraw 
        /// from the balance.
        /// </summary>
        /// <param name="sum">Sum to withdraw</param>
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
