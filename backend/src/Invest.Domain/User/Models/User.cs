using System;

namespace Invest.Domain.User.Models
{
    public class User
    {
        public User() { }
        public User(Guid uuid, string UserName, string Password, string FirstName, string LastName, string Email) 
        { 
            this.UUID = uuid;
            this.UserName = UserName;
            this.Password = Password;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Created = DateTime.UtcNow;
        }

        public Guid UUID{ get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
    }
}
