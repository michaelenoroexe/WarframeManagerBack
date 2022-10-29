namespace UserValidation
{
    public interface IUserConverter<T>
    {
        public IClientUser CreateUser(T user);
    }
}
