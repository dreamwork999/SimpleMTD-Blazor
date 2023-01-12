using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplyMTD.Models.MTD
{
    [Table("Client", Schema = "dbo")]
    public partial class Client
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string VATNumber { get; set; }

        public string NextTask { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? Deadline { get; set; }

        public string Manager { get; set; }

        public string Partner { get; set; }

        public string Authorisation { get; set; }

        public string Subscription { get; set; }

        public string Note { get; set; }

        public string UserId { get; set; }

    }
}