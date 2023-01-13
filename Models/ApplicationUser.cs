using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace SimplyMTD.Models
{
    public partial class ApplicationUser : IdentityUser
    {

        [JsonIgnore, IgnoreDataMember]
        public override string PasswordHash { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }

        [JsonIgnore, IgnoreDataMember, NotMapped]
        public string Name
        {
            get
            {
                return UserName;
            }
            set
            {
                UserName = value;
            }
        }

        public string BusinessName { get; set; }

		public string BusinessType { get; set; }

		public string Nino { get; set; }

        public string Vrn { get; set; }

        public string Address { get; set; }

        public string Photo { get; set; }

		public ICollection<ApplicationRole> Roles { get; set; }
    }
}