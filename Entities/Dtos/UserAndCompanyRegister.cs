using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserAndCompanyRegister
    {
       public UserForRegister userForRegister { get; set; }
        public Company company { get; set; }
    }
}
