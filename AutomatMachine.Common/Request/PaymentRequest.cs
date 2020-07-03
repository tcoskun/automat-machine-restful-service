using AutomatMachine.Common.Types;

namespace AutomatMachine.Common.Request
{
    public class PaymentRequest
    {
        public PaymentType PaymentType { get; }
        public decimal Amount { get; }

        public PaymentRequest(PaymentType paymentType, decimal amount)
        {
            PaymentType = paymentType;
            Amount = amount;
        }
    }
}
