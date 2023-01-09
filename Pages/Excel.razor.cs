using Newtonsoft.Json;
using Radzen.Blazor;
using Radzen;
using SimplyMTD.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SimplyMTD.Models.MTD;
using DocumentFormat.OpenXml.Wordprocessing;
using System;

namespace SimplyMTD.Pages
{
	public partial class Excel
	{
		[Inject]
		protected IJSRuntime JSRuntime { get; set; }

		[Inject]
		public NotificationService NotificationService { get; set; }

		[Inject]
		public VATService VATService { get; set; }

		[Parameter]
		public dynamic periodKey { get; set; }

		protected override async Task OnInitializedAsync()
		{
			vatReturn = new SimplyMTD.Models.VATReturn();
		}

		protected SimplyMTD.Models.VATReturn vatReturn;

		int progress;
		bool showProgress;
		bool showComplete;
		string completionMessage;
		bool cancelUpload = false;


		void CompleteUpload(UploadCompleteEventArgs args)
		{
			if (!args.Cancelled)
				completionMessage = "Upload Complete!";
			else
				completionMessage = "Upload Cancelled!";

			showProgress = false;
			showComplete = true;
		}

		void TrackProgress(UploadProgressArgs args)
		{
			showProgress = true;
			showComplete = false;
			progress = args.Progress;

			// cancel upload
			args.Cancel = cancelUpload;

			// reset cancel flag
			cancelUpload = false;
		}

		void CancelUpload()
		{
			cancelUpload = true;
		}


		void OnProgress(UploadProgressArgs args, string name)
		{
			Console.WriteLine(name);
			if (args.Progress == 100)
			{
				foreach (var file in args.Files)
				{
				}
			}
		}

		void OnComplete(UploadCompleteEventArgs args)
		{
			List<string> amounts = JsonConvert.DeserializeObject<List<string>>(args.RawResponse);
			this.vatReturn.periodKey = periodKey;
			this.vatReturn.vatDueSales = float.Parse(amounts.ElementAt(1));
			this.vatReturn.vatDueAcquisitions = float.Parse(amounts.ElementAt(2));
			this.vatReturn.totalVatDue = float.Parse(amounts.ElementAt(3));
			this.vatReturn.vatReclaimedCurrPeriod = float.Parse(amounts.ElementAt(4));
			this.vatReturn.netVatDue = float.Parse(amounts.ElementAt(5));
			this.vatReturn.totalValueSalesExVAT = int.Parse(amounts.ElementAt(6));
			this.vatReturn.totalValuePurchasesExVAT = int.Parse(amounts.ElementAt(7));
			this.vatReturn.totalValueGoodsSuppliedExVAT = int.Parse(amounts.ElementAt(8));
			this.vatReturn.totalAcquisitionsExVAT = int.Parse(amounts.ElementAt(9));
			this.vatReturn.finalised = true;
		}

		protected async Task FormSubmit()
		{
			var test = vatReturn.periodKey;
			bool res = await VATService.submitVAT(vatReturn);
			if(res)
			{
				ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success Summary", Detail = "Success Detail", Duration = 4000 });
			} else
			{
				ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error Summary", Detail = "Error Detail", Duration = 4000 });
			}
		}

		void Cancel()
		{
			//
		}

		void ShowNotification(NotificationMessage message)
		{
			NotificationService.Notify(message);

		}
	}
}