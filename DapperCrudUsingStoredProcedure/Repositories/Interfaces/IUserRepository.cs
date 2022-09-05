using DapperCrudUsingStoredProcedure.Models;

namespace DapperCrudUsingStoredProcedure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserModel> GetUserByNameEmail(string name);
        public Task<IEnumerable<UserModel>> GetAllUsers();
        public Task<int> RegisterUser(UserModel user);
        public Task<int> UpdateUser(UserModel user);
        public Task<int> DeleteUser(int id);
    }
}
