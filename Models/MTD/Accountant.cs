using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplyMTD.Models.MTD
{
	[Table("Accountant", Schema = "dbo")]
	public partial class Accountant
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		public string Email { get; set; }

		public string Permissions { get; set; }

		public string UserId { get; set; }
	}
}
