using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Invest.API.ViewModels
{
    public class LoginRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>userName</example>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>my_password</example>
        public string Password { get; set; }
    }
}
