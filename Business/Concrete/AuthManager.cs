
using Business.Abstract;
using Business.Constans;
using Core.Entities.Concrete;
using Core.Utilities.Hashing;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Core.Utilities.Security.JWT;
using Entities.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly  ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }


        public IDataResult<AccessToken> CreateAccsesToken(User user, int companyId)
        {
            var claims = _userService.GetClaims(user, companyId);
            var accessToken = _tokenHelper.CreateToken(user, claims, companyId);
            return new SuccessDataResult<AccessToken>(accessToken);
        }

        public IDataResult<User> Login(UserForLogin userForLogin)
        {
            var userToCheck = _userService.GetByMail(userForLogin.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Message.UserNotFound);
            }
            if(!HashingHelper.VerifyPasswordHash(userForLogin.Password, userToCheck.PasswordHash,userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Message.PasswordError);
            }
            return new SuccessDataResult<User>(userToCheck,Message.SuccessfulLogin);
        }

        public IDataResult<User> Register(UserForRegister userForRegister, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password,out passwordHash,out  passwordSalt);
            var user = new User()
            {
                Email = userForRegister.Email,
                AddedAt = DateTime.Now,
                IsActive = true,
                MailConfirm = false,
                MailConfirmDate = DateTime.Now,
                MailConfirmValue = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Name = userForRegister.Name,
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, Message.UserRegistered);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email)!= null)
            {
                return new ErrorResult(Message.UserAlreadyExist);

            }
            return new SuccessResult();
        }
    }
}
