using eLibrary.DAL;

namespace eLibrary.BLL
{

    public class UserRepository
    {
        DAL.AppContext db = new DAL.AppContext();
        public UserRepository(DAL.AppContext db)
        {
            this.db = db;
        }

        public User GetUser(int id)
        {
            return db.Users.FirstOrDefault(u => u.Id == id);
        }

        public List<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        public void AddUser(User user)
        {
            db.Users.Add(user);
        }

        public bool DeleteUser(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            db.Users.Remove(user);
            // учесть удаление из связанной таблицы и проверку, что все ок
            return true;
        }

        public void UpdateUserName(int id, string? name)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            user.Name = name;
            db.SaveChanges();
        }
    }
}