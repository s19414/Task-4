namespace APBD4.Models
{
    [Serializable]
    public class Animal
    {
        public Animal(int idAnimal, string Name, string Description, string Category, string Area)
        {
            this.idAnimal = idAnimal;
            this.Name = Name;
            this.Description = Description;
            this.Category = Category;
            this.Area = Area;
        }

        public int idAnimal { get; set; }
        public string Name { get; set;}
        public string Description { get; set;}
        public string Category { get; set;}
        public string Area { get; set;}

    }
}
