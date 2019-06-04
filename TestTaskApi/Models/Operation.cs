using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskApi.Models
{
    public class Operation
    {
        public OperationType OperationType { get; set; }

        public decimal Sum { get; set; }
    }
}
