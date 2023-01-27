using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplyMTD.Models.MTD
{
    [Table("AspNetUsers", Schema = "dbo")]
    public partial class AspNetUser
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public int AccessFailedCount { get; set; }

        public string ConcurrencyStamp { get; set; }

        public string Email { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }

        [Required]
        public bool LockoutEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public string NormalizedEmail { get; set; }

        public string NormalizedUserName { get; set; }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public bool PhoneNumberConfirmed { get; set; }

        public string SecurityStamp { get; set; }

        [Required]
        public bool TwoFactorEnabled { get; set; }

        public int ClientId { get; set; }

        public string UserName { get; set; }

        public string Vrn { get; set; }

        public string BusinessName { get; set; }

        public string OwnerName { get; set; }

        public string Address { get; set; }

        public string Address2 { get; set; }

        public string PostCode { get; set; }

        public string Nino { get; set; }

        public string BusinessType { get; set; }

        public string Photo { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public DateTime? Deadline { get; set; }

    }
}