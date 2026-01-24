namespace UniHub.Dto
{
    public class StoreItemReportDto
    {
        public string StoreLocation { get; set; }
        public Dictionary<string, int> Items { get; set; }
    }
}