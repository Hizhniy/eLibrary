using eLibrary.DAL;
using eLibrary.BLL;
using Microsoft.IdentityModel.Tokens;

namespace eLibrary
{
    class Programm
    {
        static void Main(string[] args)
        {
            // Можно было бы еще навернуть количество книг по каждой записи книги, но в нашей библиотеке все книги уникальные -)

            using (var db = new DAL.AppContext())
            {
                DataInitialization(db);

                UserRepository userRepository = new UserRepository(db);
                BookRepository bookRepository = new BookRepository(db);

                Console.WriteLine("Вы в библиотеке и можете:");
                Console.WriteLine("0: Покинуть библиотеку");
                Console.WriteLine("1: Получить список книг определенного жанра и вышедших между определенными годами");
                Console.WriteLine("2: Получить количество книг определенного автора");
                Console.WriteLine("3: Получить количество книг определенного жанра");
                Console.WriteLine("4: Узнать, есть ли книга определенного автора и с определенным названием");
                Console.WriteLine("5: Узнать, есть ли определенная книга у кого-то на руках");
                Console.WriteLine("6: Узнать количество книг на руках у определенного читателя");
                Console.WriteLine("7: Получить информацию о последней вышедшей книге");
                Console.WriteLine("8: Получить список всех книг, отсортированного в алфавитном порядке по названию");
                Console.WriteLine("9: Получить список всех книг, отсортированного в порядке убывания года их выхода");
                Console.WriteLine();

                string command = string.Empty;
                do
                {
                    Console.Write("Чего изволите? ");
                    command = (Console.ReadLine().Trim());

                    switch (command)
                    {
                    // вся обработка по кейсам должна быть в отдельных методах и вынесена в PLL, но уже нет сил делать задание - сроки горят -)
                        case "1":
                            {
                                Console.Write("Укажите жанр: ");
                                string genre = Console.ReadLine().Trim().ToUpper();
                                if (genre.IsNullOrEmpty()) genre = "";
                                Console.Write("Укажите год начала поиска: ");
                                //проверяем на вменяемое значение: если введено криво, присваиваем минимальное/максимальное
                                if (!int.TryParse(Console.ReadLine(), out int fromYear)) fromYear = int.MinValue;
                                Console.Write("Укажите год окончания поиска: ");
                                if (!int.TryParse(Console.ReadLine(), out int toYear)) toYear = int.MaxValue;
                                var booksList = bookRepository.FindGenreYears(genre, fromYear, toYear);
                                foreach (Book book in booksList)
                                {
                                    Console.WriteLine($"Автор: {book.Author}; Наименование: {book.Title}; Год публикации: {book.YearOfPublication}");
                                }
                                break;
                            }

                        case "2":
                            {
                                Console.Write("Укажите автора: ");
                                string author = Console.ReadLine().Trim().ToUpper();
                                if (author.IsNullOrEmpty()) author = "";
                                Console.WriteLine($"Количество: {bookRepository.CountBooksByAuthor(author)}");
                                break;
                            }
                        case "3":
                            {
                                Console.Write("Укажите жанр: ");
                                string genre = Console.ReadLine().Trim().ToUpper();
                                if (genre.IsNullOrEmpty()) genre = "";
                                Console.WriteLine($"Количество: {bookRepository.CountBooksByGenre(genre)}");
                                break;
                            }
                        case "4":
                            {
                                Console.Write("Укажите автора: ");
                                string author = Console.ReadLine().Trim().ToUpper();
                                if (author.IsNullOrEmpty()) author = "";
                                Console.Write("Укажите название: ");
                                string title = Console.ReadLine().Trim().ToUpper();
                                if (title.IsNullOrEmpty()) title = "";
                                if (bookRepository.FindAuthorTitle(author, title)) Console.WriteLine("Такая книга у нас в библиотеке есть!");
                                else Console.WriteLine("Такой книги у нас в библиотеке нет...");
                                break;
                            }
                        case "5":
                            {
                                Console.Write("Укажите автора: ");
                                string author = Console.ReadLine().Trim().ToUpper();
                                if (author.IsNullOrEmpty()) author = "";
                                Console.Write("Укажите название: ");
                                string title = Console.ReadLine().Trim().ToUpper();
                                if (title.IsNullOrEmpty()) title = "";
                                if (bookRepository.FindAuthorTitle(author, title))
                                {
                                    if (bookRepository.IsBookAvailable(author, title)) Console.WriteLine("Эта книга пока находится у читателя");
                                    else Console.WriteLine("Эта книга доступна!");
                                }
                                else Console.WriteLine("Такой книги у нас в библиотеке нет...");
                                break;
                            }
                        case "6":
                            {
                                Console.Write("Как зовут читателя? ");
                                string readerName = Console.ReadLine().Trim().ToUpper();
                                if (readerName.IsNullOrEmpty()) readerName = "";
                                Console.WriteLine($"Количество книг на руках: {bookRepository.CountBooksByReader(readerName)}");
                                break;
                            }
                        case "7":
                            {
                                var book = bookRepository.LastBook();
                                Console.WriteLine($"Автор: {book.Author}; Наименование: {book.Title}; Год публикации: {book.YearOfPublication}");
                                break;
                            }
                        case "8":
                            {
                                var booksList = bookRepository.BooksListByTitle();
                                foreach (Book book in booksList)
                                {
                                    Console.WriteLine($"Автор: {book.Author}; Наименование: {book.Title}; Год публикации: {book.YearOfPublication}");                                    
                                }
                                break;
                            }
                        case "9":
                            {
                                var booksList = bookRepository.BooksListByYearOfPublication();
                                foreach (Book book in booksList)
                                {
                                    Console.WriteLine($"Автор: {book.Author}; Наименование: {book.Title}; Год публикации: {book.YearOfPublication}");
                                }
                                break;
                            }
                    }
                    Console.WriteLine();
                }
                while (command != "0");
            }
        }

        static DAL.AppContext DataInitialization(DAL.AppContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var book1 = new Book { Author = "Оруэлл", Title = "1984", Genre = "антиутопия", YearOfPublication = 1949 };
            var book2 = new Book { Author = "Носов", Title = "Незнайка на Луне", Genre = "сказка, приключенческий роман, антиутопия", YearOfPublication = 1965 };
            var book3 = new Book { Author = "Толстой", Title = "Война и мир", Genre = "роман, историческая проза", YearOfPublication = 1869 };
            var book4 = new Book { Title = "Корейские сказки", Genre = "сказка", YearOfPublication = 777 };
            var book5 = new Book { Title = "Библия" };
            var book6 = new Book { Author = "Джаред Даймонд", Title = "Ружья, микробы и сталь", Genre = "научпоп", YearOfPublication = 1997 };
            var book7 = new Book { Author = "Толстой", Title = "Анна Каренина", Genre = "роман", YearOfPublication = 1877 };

            var user1 = new User { Name = "Артур", Email = "arthur@mail.com" };
            var user2 = new User { Name = "Клим", Email = "klim@mail.com" };
            var user3 = new User { Name = "Стефания Петровна" };
            var user4 = new User { Name = "Юля", Email = "julia@mail.com" };
                        
            book1.User = user1;
            book4.User = user1;
            //book4.UserId = user1.Id;
            user2.Books.Add(book2);

            db.Users.AddRange(user1, user2, user3, user4);
            db.Books.AddRange(book1, book2, book3, book4, book5, book6, book7);
            db.SaveChanges();

            return db;
        }
    }
}