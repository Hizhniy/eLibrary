namespace eLibrary.DAL
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; } 

        //Навигационное свойство
        public List<Book> Books { get; set; } = new List<Book>();
    }
}