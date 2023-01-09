using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Client;
using Microsoft.JSInterop;
using Radzen;
using SimplyMTD.Models;
using SimplyMTD.Models.MTD;

namespace SimplyMTD.Pages
{
    public partial class Setting
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

        protected string oldPassword = "";
        protected string newPassword = "";
        protected string confirmPassword = "";
        
        protected string error;
        protected bool errorVisible;
        protected bool successVisible;

        [Inject]
        protected SecurityService Security { get; set; }
        protected SimplyMTD.Models.ApplicationUser user;
        [Inject]
        public MTDService MTDService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            user = await Security.GetUserById($"{Security.User.Id}");
        }

        protected async Task FormSubmit()
        {
            try
            {
                await Security.ChangePassword(oldPassword, newPassword);
                successVisible = true;
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }

        void Cancel()
        {
            //
        }
    }
}