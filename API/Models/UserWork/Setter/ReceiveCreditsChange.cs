namespace API.Models.UserWork.Setter
{
    /// <summary>
    /// Projection of user credits number change request data.
    /// </summary>
    public sealed class ReceiveCreditsChange
    {
        /// <summary>
        /// New credits number.
        /// </summary>
        public int Number { get; }
        /// <summary>
        /// Initialize credits number.
        /// </summary>
        public ReceiveCreditsChange(int number) => Number = number;
    }
}
