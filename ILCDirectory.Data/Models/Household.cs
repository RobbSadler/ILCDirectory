namespace ILCDirectory.Data.Models
{
    public class Household
    {
        public int HouseholdId {  get; set; }
        public string HouseholdName { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
    }
}
