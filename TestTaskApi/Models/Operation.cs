namespace TestTaskApi.Models
{
    /// <summary>
    /// Entity that helps to realize 
    /// Add/Withdraw operations with the balance
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// Operation type to realize
        /// </summary>
        public OperationType OperationType { get; set; }

        /// <summary>
        /// Amount of money 
        /// </summary>
        public decimal Sum { get; set; }
    }
}
