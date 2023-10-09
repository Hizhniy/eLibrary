namespace eLibrary.DAL
{
    public class Book
    {
        public int Id { get; set; }
        public string? Author { get; set; }
        public string Title { get; set; } = null!;
        public string? Genre { get; set; } = null!;
        public int? YearOfPublication { get; set; }
                
        //Внешний ключ
        public int? UserId { get; set; }
        //Навигационное свойство
        public User User { get; set; } = null!;

        //public List<User> Users { get; set; } = new List<User>(); // многие ко многим
    }
}