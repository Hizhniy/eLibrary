using eLibrary.DAL;
using Microsoft.IdentityModel.Tokens;


namespace eLibrary.BLL
{
    public class BookRepository
    {
        private readonly DAL.AppContext db;// = new DAL.AppContext();
        public BookRepository(DAL.AppContext db)
        {
            this.db = db;
        }

        public Book GetBook(int id)
        {
            var book = db.Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return new Book();
            else return book;
        }

        public List<Book> GetAllBooks()
        {
            return db.Books.ToList();
        }

        public void AddBook(Book book)
        {
            db.Books.Add(book);
            db.SaveChanges();
        }

        public bool DeleteBook(int id)
        {
            var book = db.Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return false;
            else
            {
                db.Books.Remove(book);
                db.SaveChanges();
                // учесть удаление из связанной таблицы и проверку, что все ок
                return true;
            }
        }

        public bool UpdateBookYearOfPublication(int id, int? year)
        {
            var book = db.Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return false;
            else
            {
                book.YearOfPublication = year;
                db.SaveChanges();
                return true;
            }
        }

        public List<Book> FindGenreYears(string genre, int fromYear, int toYear)
        {
            return db.Books.Where(b => (b.Genre.ToUpper().Contains(genre)) && (b.YearOfPublication >= fromYear) && (b.YearOfPublication <= toYear)).ToList();
            //return (from book in db.Books
            //       where book.Genre.Contains(genre) && book.YearOfPublication >= fromYear && book.YearOfPublication <= toYear
            //       select book).ToList();
        }

        public int CountBooksByAuthor(string author)
        {
            return db.Books.Where(b => b.Author.ToUpper() == author).Count();
        }

        public int CountBooksByGenre(string genre)
        {
            return db.Books.Where(b => b.Genre.ToUpper().Contains(genre)).Count();
        }

        public bool FindAuthorTitle(string author, string title)
        {
            var isBook = db.Books.FirstOrDefault(b => (b.Author.ToUpper() == author) && (b.Title.ToUpper() == title));
            if (isBook == null) return false;
            else return true;
        }

        public bool IsBookAvailable(string author, string title)
        {
            var isBook = db.Books.FirstOrDefault(b => (b.Author.ToUpper() == author) && (b.Title.ToUpper() == title));
            if (isBook.UserId == null) return false;
            else return true;
        }

        public int CountBooksByReader(string readerName)
        {
            var readersList = db.Users.Where(u => u.Name.ToUpper() == readerName).Select(u => u.Id).ToList();
            if (readersList.IsNullOrEmpty()) return 0;
            else return db.Books.Where(b => b.UserId == readersList[0]).Count();
        }

        public Book LastBook()
        {
            var booksList = db.Books.OrderByDescending(b => b.YearOfPublication).ToList();
            return booksList[0];
        }

        public List<Book> BooksListByTitle()
        {
            return db.Books.OrderBy(b => b.Title).ToList();
        }
        public List<Book> BooksListByYearOfPublication()
        {
            return db.Books.OrderByDescending(b => b.YearOfPublication).ToList();
        }
    }
}