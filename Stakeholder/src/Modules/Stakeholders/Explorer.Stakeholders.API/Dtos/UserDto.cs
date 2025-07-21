using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
      //  public UserRole Role { get; private set; }
        public bool IsActive { get; set; }

        public UserDto(string username)
        {
            Username = username;
        }

    }
   /* public enum UserRole
    {
        Administrator,
        Author,
        Tourist
    }*/
}
