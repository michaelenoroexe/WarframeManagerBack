namespace UserValidation
{
    public interface IClientUser
    {
        string Login { get; }
        string? Password { get; }
    }
}
