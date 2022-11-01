namespace API.Models.UserWork.Setter
{
    /// <summary>
    /// Projection of user resource number change request data.
    /// </summary>
    public sealed class ReceiveResourceChange
    {
        /// <summary>
        /// Id for resource to change.
        /// </summary>
        public string Resource { get; }
        /// <summary>
        /// New number of resource.
        /// </summary>
        public int Number { get; }
        /// <summary>
        /// Type of resource.
        /// </summary>
        public string Type { get; }

        public ReceiveResourceChange(string resource, int number, string type)
        {
            Resource = resource;
            Number = number;
            Type = type;
        }
    }
}
