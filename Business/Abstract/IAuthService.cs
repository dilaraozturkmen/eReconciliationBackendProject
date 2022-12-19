
using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Security.JWT;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegister userForRegister, string password);
        IDataResult<User> Login(UserForLogin userForLogin);
        IResult UserExists(string email);
        IDataResult<AccessToken> CreateAccsesToken(User user, int companyId);

    }
}
