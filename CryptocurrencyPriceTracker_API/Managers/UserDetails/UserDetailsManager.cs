using CryptocurrencyPriceTracker_API.Entity;
using CryptocurrencyPriceTracker_API.Interfaces;
using CryptocurrencyPriceTracker_API.Model.User;
using CryptocurrencyPriceTracker_API.Repositories;
using System;
using System.Collections.Generic;
using CryptocurrencyPriceTracker_API.Helper;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography;

namespace CryptocurrencyPriceTracker_API.Managers
{
    public class UserDetailsManager:IUserDetailsManager
    {
        private readonly IUserDetailsRepository _userDetailsRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public UserDetailsManager(IUserDetailsRepository userDetailsRepository, IConfiguration configuration, IEmailService emailService)
        {
            _userDetailsRepository = userDetailsRepository;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<TokenDtoModel> AuthenticateUser(UserDetailModel model)
        {
            var entity = await _userDetailsRepository.GetUserDetail(model.UserName);
            if (entity == null)
                return null;
            else
            {
                if (!PasswordHasher.VerifyPassword(model.Password, entity.Password))
                    throw new Exception ("Wrong Password!");
                entity.Token= CreateJwt(entity);
                TokenDtoModel tokenDto = new TokenDtoModel();
                tokenDto.RefreshToken =CreateRefreshToken();
                tokenDto.AccessToken = entity.Token;
                return tokenDto;
            }
        }

        public async Task RegisterUser(UserDetailModel model)
        {
            if (await CheckUserNameExist(model.UserName))
                throw new Exception("UserName Already Exist");
            if (await CheckEmailExist(model.Email))
                throw new Exception("Email Already Exist");

            model.Password = PasswordHasher.HashPassword(model.Password);
            _userDetailsRepository.RegisterUser(model.ToEntity());
            await _userDetailsRepository.SaveChangesAsync();
        }
        private async Task<bool> CheckUserNameExist(string userName)
        {
          return await _userDetailsRepository.CheckUserNameExist(userName);
        }
        private async Task<bool> CheckEmailExist(string email)
        {
            return await _userDetailsRepository.CheckEmailExist(email);
        }
        public string CheckPasswordStregth(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < 8)
                sb.Append("Minimum password length should be 8" + Environment.NewLine);
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]")
                && Regex.IsMatch(password, "[0-9]")))
                sb.Append("Password should be Alphanumeric" + Environment.NewLine);
            if (!Regex.IsMatch(password, "[!,@,#,$,%,^,&,*,|,\\,~,`,),(,-,_,=.+]"))
                sb.Append("Password should contain special chars" + Environment.NewLine);
            return sb.ToString();
        }

        public string CreateJwt(UserDetailEntity user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysceret......");
            string role;
            if (user.Role == 1)
                role = "Admin";
            else
                role = "User";
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role,role),
                new Claim(ClaimTypes.Name,$"{user.UserName}")
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        public async Task<IEnumerable<UserDetailModel>> GetUserDetailsList()
        {
            IEnumerable<UserDetailEntity> entities =await _userDetailsRepository.GetUserDetailsList();
            return entities.Select(x=>x.ToModel()).ToList();
        }
        public string CreateRefreshToken()
        {
            byte[] number = new byte[4];
            RandomNumberGenerator.Create().GetBytes(number);
            return Convert.ToBase64String(number);

        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryverysceret......")),
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime=false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principle = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if(jwtSecurityToken==null||!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("This is Invalid Token");
            }
            return principle;

        }

        public async Task<UserDetailEntity> GetUserDetail(string username)
        {
            return await _userDetailsRepository.GetUserDetail(username);
        }

        public async Task<UserDetailEntity> GetUserDetailUsingEmail(string email)
        {
            var entity = await _userDetailsRepository.GetUserDetailUsingEmail(email);
            if (entity == null)
                return null;
            else
            {
                return entity;
            }
        }
        public async Task<UserDetailEntity> UpdateUserPassword(UserDetailEntity entity, string email)
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            entity.ResetPasswordToken = emailToken;
            entity.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            string from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset Password!!", EmailBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);
            _userDetailsRepository.UpdateUserPassword(entity);
            await _userDetailsRepository.SaveChangesAsync();
            return entity;
        }
        public async Task<UserDetailEntity> ResetUserPassword(ResetPasswordModel model, UserDetailEntity entity)
        {
            entity.Password = PasswordHasher.HashPassword(model.NewPassword);
            _userDetailsRepository.UpdateUserPassword(entity);
            await _userDetailsRepository.SaveChangesAsync();
            return entity;
        }
    }
}
