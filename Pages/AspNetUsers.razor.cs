using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace SimplyMTD.Pages
{
	public partial class AspNetUsers
	{
		[Inject]
		protected IJSRuntime JSRuntime { get; set; }

		[Inject]
		protected NavigationManager NavigationManager { get; set; }

		[Inject]
		protected DialogService DialogService { get; set; }

		[Inject]
		protected TooltipService TooltipService { get; set; }

		[Inject]
		protected ContextMenuService ContextMenuService { get; set; }

		[Inject]
		protected NotificationService NotificationService { get; set; }

		[Inject]
		public MTDService MTDService { get; set; }

		protected IEnumerable<SimplyMTD.Models.MTD.AspNetUser> aspNetUsers;

		protected RadzenDataGrid<SimplyMTD.Models.MTD.AspNetUser> grid0;

		protected string search = "";

		protected async Task Search(ChangeEventArgs args)
		{
			search = $"{args.Value}";

			await grid0.GoToPage(0);

			aspNetUsers = await MTDService.GetAspNetUsers(new Query { Filter = $@"i => i.Id.Contains(@0) || i.ConcurrencyStamp.Contains(@0) || i.Email.Contains(@0) || i.NormalizedEmail.Contains(@0) || i.NormalizedUserName.Contains(@0) || i.PasswordHash.Contains(@0) || i.PhoneNumber.Contains(@0) || i.SecurityStamp.Contains(@0) || i.UserName.Contains(@0) || i.Vrn.Contains(@0) || i.BusinessName.Contains(@0) || i.Address.Contains(@0) || i.Nino.Contains(@0) || i.BusinessType.Contains(@0) || i.Photo.Contains(@0)", FilterParameters = new object[] { search } });
		}
		protected override async Task OnInitializedAsync()
		{
			aspNetUsers = await MTDService.GetAspNetUsers(new Query { Filter = $@"i => i.Id.Contains(@0) || i.ConcurrencyStamp.Contains(@0) || i.Email.Contains(@0) || i.NormalizedEmail.Contains(@0) || i.NormalizedUserName.Contains(@0) || i.PasswordHash.Contains(@0) || i.PhoneNumber.Contains(@0) || i.SecurityStamp.Contains(@0) || i.UserName.Contains(@0) || i.Vrn.Contains(@0) || i.BusinessName.Contains(@0) || i.Address.Contains(@0) || i.Nino.Contains(@0) || i.BusinessType.Contains(@0) || i.Photo.Contains(@0)", FilterParameters = new object[] { search } });
		}

		protected async Task AddButtonClick(MouseEventArgs args)
		{
			await DialogService.OpenAsync<AddAspNetUser>("Add AspNetUser", null);
			await grid0.Reload();
		}

		protected async Task EditRow(DataGridRowMouseEventArgs<SimplyMTD.Models.MTD.AspNetUser> args)
		{
			await DialogService.OpenAsync<EditAspNetUser>("Edit AspNetUser", new Dictionary<string, object> { { "Id", args.Data.Id } });
		}

		protected async Task GridDeleteButtonClick(MouseEventArgs args, SimplyMTD.Models.MTD.AspNetUser aspNetUser)
		{
			try
			{
				if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
				{
					var deleteResult = await MTDService.DeleteAspNetUser(aspNetUser.Id);

					if (deleteResult != null)
					{
						await grid0.Reload();
					}
				}
			}
			catch (Exception ex)
			{
				NotificationService.Notify(new NotificationMessage
				{
					Severity = NotificationSeverity.Error,
					Summary = $"Error",
					Detail = $"Unable to delete AspNetUser"
				});
			}
		}

		protected async Task ExportClick(RadzenSplitButtonItem args)
		{
			if (args?.Value == "csv")
			{
				await MTDService.ExportAspNetUsersToCSV(new Query
				{
					Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
					OrderBy = $"{grid0.Query.OrderBy}",
					Expand = "",
					Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible()).Select(c => c.Property))
				}, "AspNetUsers");
			}

			if (args == null || args.Value == "xlsx")
			{
				await MTDService.ExportAspNetUsersToExcel(new Query
				{
					Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
					OrderBy = $"{grid0.Query.OrderBy}",
					Expand = "",
					Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible()).Select(c => c.Property))
				}, "AspNetUsers");
			}
		}
	}
}