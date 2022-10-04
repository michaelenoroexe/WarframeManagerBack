using API.Models;

namespace API.Repositories
{
    public static class UserResourcesChangesBuffer
    {
        public static LinkedList<UserResourcesChanges> _totalBuffer = new();
    }
}
