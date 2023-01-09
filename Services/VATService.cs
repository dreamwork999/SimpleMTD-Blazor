using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using SimplyMTD.Data;
using SimplyMTD.Models;
using System.Net.Http.Headers;

namespace SimplyMTD
{
	public partial class VATService
	{
		private readonly IConfiguration configuration;
		private readonly TokenProvider _store;
		private readonly string token;

        public async Task<bool> HmrcAsync()
        {
            if (this.token != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public VATService(TokenProvider tokenProvider, IConfiguration configuration)
		{
			this._store = tokenProvider;
			this.configuration = configuration;
			this.token = _store.AccessToken;
		}

		public async Task<List<Obligation>> GetObligations()
		{
			var token = this.token; // Todo

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(configuration.GetValue<string>("Auth0:uri"));
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.hmrc.1.0+json"));
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				HttpResponseMessage response = await client.GetAsync("organisations/vat/423439757/obligations?from=2023-01-01&to=2023-01-04&status=O");

				String resp = await response.Content.ReadAsStringAsync();
				ObligationBody obligationBody = JsonConvert.DeserializeObject<ObligationBody>(resp);
				return obligationBody.obligations;
			}
		}

		public async Task<bool> submitVAT(VATReturn vatReturn)
		{
            var token = this.token; // Todo
			 
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("Auth0:uri"));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.hmrc.1.0+json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				vatReturn.finalised = true;

                var response = await client.PostAsJsonAsync("organisations/vat/423439757/returns", vatReturn);
				// Todo: customize
				if((int)response.StatusCode == 201)
				{
					return true;
				} else
				{
					return false;
				}
				
				

            }
        }
    }
}
