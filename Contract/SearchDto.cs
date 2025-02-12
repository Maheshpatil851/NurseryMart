namespace NurseryMart.Contract
{
    public class SearchDto 
    {
        public string? Query { set; get; }
        public Pagination? Pagination { set; get; }
    }
}
