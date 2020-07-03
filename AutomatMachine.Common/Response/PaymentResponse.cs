using AutomatMachine.Common.Types;

namespace AutomatMachine.Common.Response
{
    public class PaymentResponse
    {
        public string ProductName { get; }
        public int Piece { get; }
        public PaymentType PaymentType { get; }
        public decimal ReturnAmount { get; }

        public PaymentResponse(string productName, int piece, PaymentType paymentType, decimal returnAmount)
        {
            ProductName = productName;
            Piece = piece;
            PaymentType = paymentType;
            ReturnAmount = returnAmount;
        }
    }
}
