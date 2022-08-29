using API.Models;
using MongoDB.Driver;

namespace API
{
    public static class Conv
    {
        // Convert Components ICursor to Resource list
        public static async Task<List<Resource>> ToResourceList(IAsyncCursor<Component> components)
        {
            List<Component> componentsList = await components.ToListAsync();
            return componentsList.Select(x =>
            {
                return new Resource(x.Id, x.Name, x.Type, x.Location, x.Mastery);
            }).ToList();
        }
    }
}
