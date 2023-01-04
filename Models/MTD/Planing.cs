using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplyMTD.Models.MTD
{
    [Table("Planing", Schema = "dbo")]
    public partial class Planing
    {
        [Key]
        [Required]
        public string id { get; set; }

    }
}