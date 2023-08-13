namespace ILCDirectory.Data.Models
{
    public class Family
    {
        public int FamilyId {  get; set; }
        public List<Person> Parents {  get; set; }
        public List<Person> Children {  get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
    }
}
