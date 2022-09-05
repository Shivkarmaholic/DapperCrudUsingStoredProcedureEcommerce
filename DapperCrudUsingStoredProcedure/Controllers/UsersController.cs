using DapperCrudUsingStoredProcedure.Models;
using DapperCrudUsingStoredProcedure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperCrudUsingStoredProcedure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;   
        }

        [HttpGet]
        [Route("/[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsers();
                return Ok(users);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("/[action]")]
        public async Task<IActionResult> GetGetUserByNameEmail(string searchName)
        {
            try
            {
                var user = await _userRepository.GetUserByNameEmail(searchName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("/[action]")]
        public async Task<IActionResult> RegisterUser(UserModel user)
        {
            try
            {
                var result = await _userRepository.RegisterUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPatch]
        [Route("/[action]")]
        public async Task<IActionResult> UpdateUser(UserModel user)
        {
            try
            {
                var result = await _userRepository.UpdateUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("/[action]")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userRepository.DeleteUser(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
