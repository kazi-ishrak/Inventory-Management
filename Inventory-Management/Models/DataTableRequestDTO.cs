namespace Inventory_Management.Models
{
    public class DataTableRequestDTO
    {
        public string Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public Dictionary<string, string> Search { get; set; }
        public List<Dictionary<string, string>> Columns { get; set; }
        public List<Dictionary<string, string>> Order { get; set; }
    }
}
