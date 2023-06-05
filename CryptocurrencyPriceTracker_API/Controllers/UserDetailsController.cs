using CryptocurrencyPriceTracker_API.Interfaces;
using CryptocurrencyPriceTracker_API.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyPriceTracker_API.Controllers
{
    [Route("api/UserDetails")]
    public class UserDetailsController : Controller
    {
        private readonly IUserDetailsManager _userDetailsManager;
        public UserDetailsController(IUserDetailsManager userDetailsManager)
        {
            _userDetailsManager = userDetailsManager;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserDetailModel model)
        {
            TokenDtoModel tokenDto = new TokenDtoModel();
            try
            {
                tokenDto = await _userDetailsManager.AuthenticateUser(model);
                if (tokenDto == null)
                    return BadRequest(new { Message = "User Not Found!" });
                else
                    return Ok(new TokenDtoModel
                    {
                        message = "Login Success",
                        AccessToken = tokenDto.AccessToken,
                        RefreshToken = tokenDto.RefreshToken
                    });
            }
            catch (Exception ex)
            {
               return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDetailModel model)
        {
            try
            {
                var pass = _userDetailsManager.CheckPasswordStregth(model.Password);
                if (!string.IsNullOrEmpty(pass))
                    return BadRequest(new { Message = pass });
                await _userDetailsManager.RegisterUser(model);
                return Ok(new { Message = "User Register!" });
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }          
        }
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<UserDetailModel>> GetUserDetailsList()
        {
            return await _userDetailsManager.GetUserDetailsList();
        }
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDtoModel tokenDto)
        {
            if (tokenDto is null)
                return BadRequest("Invalid Client Request");
            string accessToken = tokenDto.AccessToken;
            string refreshToken = tokenDto.RefreshToken;
            var principle = _userDetailsManager.GetPrincipalFromExpiredToken(accessToken);
            string username = principle.Identity.Name;
            var userDetail= await _userDetailsManager.GetUserDetail(username);
            if(userDetail is null)
                return BadRequest("Invalid Request");
            return Ok(new TokenDtoModel
            {
                AccessToken = _userDetailsManager.CreateJwt(userDetail),
                RefreshToken = _userDetailsManager.CreateRefreshToken()
            });

        }
    }
}
