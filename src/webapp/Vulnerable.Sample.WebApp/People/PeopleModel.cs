
namespace Vulnerable.Sample.WebApp.People 
{
    public class People 
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public People(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}