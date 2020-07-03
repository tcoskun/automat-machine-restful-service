namespace AutomatMachine.Common.Request
{
    public class SetProcessRequest
    {
        public int Piece { get; }
        public int NumberOfSugar { get; }

        public SetProcessRequest(int piece, int numberOfSugar)
        {
            Piece = piece;
            NumberOfSugar = numberOfSugar;
        }
    }
}
