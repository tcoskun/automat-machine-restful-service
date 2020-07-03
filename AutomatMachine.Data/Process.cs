using System;
using AutomatMachine.Common.Types;
using AutomatMachine.Data.Common;

namespace AutomatMachine.Data
{
    public class Process : Entity
    {
        public ProcessState State { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int ProductPiece { get; set; }
        public int NumberOfSuger { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
