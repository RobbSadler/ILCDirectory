namespace ILCDirectory.Data.Models
{
    public class Family
    {
        public int FamilyId {  get; set; }
        public string FamilyName { get; set; }
        public List<Person> Parents {  get; set; }
        public List<Person> Children {  get; set; }
    }
}
