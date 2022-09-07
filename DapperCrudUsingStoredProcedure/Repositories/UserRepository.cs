using Dapper;
using DapperCrudUsingStoredProcedure.Context;
using DapperCrudUsingStoredProcedure.Models;
using DapperCrudUsingStoredProcedure.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System.Data;

namespace DapperCrudUsingStoredProcedure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _dbContext;
        public UserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
       

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            
                List<UserModel> users = new List<UserModel>();
                using (var connection= _dbContext.CreateConnection())
                {
                    connection.Open();
                    DynamicParameters dynamicParameters = new DynamicParameters();
                    users = connection.Query<UserModel>("GetAllUserTable_SP",dynamicParameters, commandType:CommandType.StoredProcedure).ToList();
                    
                    foreach (var user in users)
                    { 
                    List<UserAddressModel> userAddresses=new List<UserAddressModel>();
                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("userid", user.Id);
                    userAddresses = connection.Query<UserAddressModel>("GetAllUserAddressTable_SP", dp, commandType: CommandType.StoredProcedure).ToList();
                    user.Address = userAddresses;
                }
                    return users;
                }
            
        }

        public async Task<UserModel> GetUserByNameEmail(string email)
        {
            UserModel user = new UserModel();
            using (var connection = _dbContext.CreateConnection())
            {
                connection.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("searchName", email);
                user =await connection.QueryFirstOrDefaultAsync<UserModel>("GetUserByNameEmail_SP", dynamicParameters, commandType: CommandType.StoredProcedure);
                                
                DynamicParameters dp = new DynamicParameters();
                dp.Add("userid", user.Id);
                var userAddresses = await connection.QueryAsync<UserAddressModel>("GetAllUserAddressTable_SP", dp, commandType: CommandType.StoredProcedure);
                user.Address = userAddresses.ToList();
                return user;
            }
        }

        public async Task<int> RegisterUser(UserModel users)
        {           
            using (var connection = _dbContext.CreateConnection())
            {
                connection.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("firstName", users.FirstName);
                dynamicParameters.Add("lastName", users.LastName);
                dynamicParameters.Add("mobileNo", users.MobileNo);
                dynamicParameters.Add("email",users.Email);
                dynamicParameters.Add("passwordd", users.Password);
                int userid = await connection.QuerySingleAsync<int>("RegisterUserTable_SP", dynamicParameters, commandType: CommandType.StoredProcedure);
                int result = await insertupdate(users.Address, userid);
                
                return result;
            }
        }

        public async Task<int> UpdateUser(UserModel user)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                connection.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("id",user.Id);
                dynamicParameters.Add("firstName", user.FirstName);
                dynamicParameters.Add("lastName", user.LastName);
                dynamicParameters.Add("mobileNo", user.MobileNo);
                dynamicParameters.Add("email", user.Email);
                dynamicParameters.Add("passwordd", user.Password);
                int result = await connection.ExecuteAsync("UpdateUserTable_SP", dynamicParameters, commandType: CommandType.StoredProcedure);

                int res = await insertupdate(user.Address, user.Id);
                //foreach (var address in user.Address)
                //{
                //    DynamicParameters dp = new DynamicParameters();
                //    dp.Add("userId", address.UserId);
                //    dp.Add("addressLine1", address.AddressLine1);
                //    dp.Add("addressLine2", address.AddressLine2);
                //    dp.Add("city", address.City);
                //    dp.Add("postalCode", address.PostalCode);
                //    dp.Add("country", address.Country);
                //    dp.Add("alternateMobile", address.AlternateMobile);
                //    result = await connection.ExecuteAsync("UpdateUserAddressTable_SP", dp, commandType: CommandType.StoredProcedure);
                //}
                return res;
            }
        }

        public async Task<int> insertupdate(List<UserAddressModel> addresses, int userid)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                int result = 0;
            UserModel us = new UserModel();
                foreach (var user in addresses)
                {
                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("userId", userid);
                    dp.Add("addressLine1", user.AddressLine1);
                    dp.Add("addressLine2", user.AddressLine2);
                    dp.Add("city", user.City);
                    dp.Add("postalCode", user.PostalCode);
                    dp.Add("country", user.Country);
                    dp.Add("alternateMobile", user.AlternateMobile);
                    result = await connection.ExecuteAsync("UpdateUserAddressTable_SP", dp, commandType: CommandType.StoredProcedure);

                }
            }
            return 0;
        }

        public async Task<int> DeleteUser(int id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                connection.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("id", id);                
                int result = await connection.ExecuteAsync("DeleteUserTable_SP", dynamicParameters, commandType: CommandType.StoredProcedure);
                result = await connection.ExecuteAsync("DeleteUserAddressTable_SP", dynamicParameters, commandType: CommandType.StoredProcedure);
                
                return result;
            }
        }
    }
}
