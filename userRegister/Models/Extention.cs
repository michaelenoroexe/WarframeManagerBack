namespace API.Models
{
    public static class Extention
    {
        public static UserInfo WithoutId(this UserInfo us)
        {
            return new UserInfo(us.Login, us.Rank, us.Image);
        }
    }
}
