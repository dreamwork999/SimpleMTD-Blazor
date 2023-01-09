using System.Net.Http;
using System.Net.Http.Headers;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Wordprocessing;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using SimplyMTD.Models;
using static System.Net.WebRequestMethods;

namespace SimplyMTD.Pages
{
    public partial class Index
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
        protected SecurityService Security { get; set; }

		[Inject]
		public VATService VATService { get; set; }

		[Inject]
		HttpClient Http { get; set; }

		protected IEnumerable<Obligation> obligations;

		protected RadzenDataGrid<Obligation> grid;

		protected override async Task OnInitializedAsync()
		{
			obligations = await VATService.GetObligations();
		}

		async Task Submit(MouseEventArgs args, string periodKey)
        {
			var confirmationResult = await this.DialogService.Confirm("How would you like to provide your VAT information?", "Dialog Title", new ConfirmOptions { OkButtonText = "EXCEL BRIDGING", CancelButtonText = "ACCOUNTING RECORDS" });
			if (confirmationResult == true) // excel bridging
			{
                NavigationManager.NavigateTo("/excel/"+periodKey);

            } else // accounting records
            {

            }
		}

		protected async System.Threading.Tasks.Task Button0Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            NavigationManager.NavigateTo("/Account/UserRestrictedCall");
        }
    }
}