namespace API.Models.Responses
{
    public sealed class GetDataResponses
    {
        public int Code { get; }
        public List<Item>? Data { get; }
        public GetDataResponses(int code, List<Item>? data = null)
        {
            Code = code;
            Data = data;
        }
    }
}
