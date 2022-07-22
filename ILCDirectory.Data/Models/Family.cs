namespace ILCDirectory.Data.Models
{
    public class Family
    {
        public int FamilyId {  get; set; }
        public string Name { get; set; }
        public List<Person> Parents {  get; set; }
        public List<Person> Children {  get; set; }
        public int DDDId { get; set; }
    }
}
