namespace API.Models.UserWork.Setter
{
    public sealed class ReceiveCreditsChange
    {
        public int Number { get; }

        public ReceiveCreditsChange(int number) => Number = number;
    }
}
