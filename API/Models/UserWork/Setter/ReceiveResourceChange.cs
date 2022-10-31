namespace API.Models.UserWork.Setter
{
    public sealed class ReceiveResourceChange
    {
        public string Resource { get; }
        public int Number { get; }
        public string Type { get; }

        public ReceiveResourceChange(string resource, int number, string type)
        {
            Resource = resource;
            Number = number;
            Type = type;
        }
    }
}
