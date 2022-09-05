namespace API.Models.Responses
{
    public class GetDataResponses
    {
        public int Code { get; set; }
        public List<Item>? Data { get; set; }
        public GetDataResponses(int code, List<Item>? data = null)
        {
            Code = code;
            Data = data;
        }
    }
}
