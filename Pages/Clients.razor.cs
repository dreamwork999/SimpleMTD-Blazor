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
    public partial class Clients
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

        protected IEnumerable<SimplyMTD.Models.MTD.Client> clients;

        protected RadzenDataGrid<SimplyMTD.Models.MTD.Client> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            clients = await MTDService.GetClients(new Query { Filter = $@"i => i.Id.Contains(@0) || i.ClientId.Contains(@0) || i.ClientName.Contains(@0) || i.VATNumber.Contains(@0) || i.NextTask.Contains(@0) || i.Manager.Contains(@0) || i.Partner.Contains(@0) || i.Authorisation.Contains(@0) || i.Subscription.Contains(@0) || i.Note.Contains(@0) || i.UserId.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            clients = await MTDService.GetClients(new Query { Filter = $@"i => i.Id.Contains(@0) || i.ClientId.Contains(@0) || i.ClientName.Contains(@0) || i.VATNumber.Contains(@0) || i.NextTask.Contains(@0) || i.Manager.Contains(@0) || i.Partner.Contains(@0) || i.Authorisation.Contains(@0) || i.Subscription.Contains(@0) || i.Note.Contains(@0) || i.UserId.Contains(@0)", FilterParameters = new object[] { search } });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddClient>("Add Client", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<SimplyMTD.Models.MTD.Client> args)
        {
            await DialogService.OpenAsync<EditClient>("Edit Client", new Dictionary<string, object> { {"Id", args.Data.Id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, SimplyMTD.Models.MTD.Client client)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await MTDService.DeleteClient(client.Id);

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
                    Detail = $"Unable to delete Client" 
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await MTDService.ExportClientsToCSV(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible()).Select(c => c.Property))
}, "Clients");
            }

            if (args == null || args.Value == "xlsx")
            {
                await MTDService.ExportClientsToExcel(new Query
{ 
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", 
    OrderBy = $"{grid0.Query.OrderBy}", 
    Expand = "", 
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible()).Select(c => c.Property))
}, "Clients");
            }
        }
    }
}