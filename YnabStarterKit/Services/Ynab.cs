using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YnabStarterKit.Models;

namespace YnabStarterKit.Services
{
    public class Ynab
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;

        public Ynab(IConfiguration config,
            IHttpClientFactory clientFactory)
        {
            this._config = config;
            this._clientFactory = clientFactory;
        }

        public async Task<YnabBudgetData> GetBudgets(string accessToken)
        {
            var httpClient = GetHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get,"budgets");
            request.Headers.Add("Authorization", "Bearer " + accessToken);

            using (var response = await httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<YnabBudgetData>(data);
            }
        }

        public async Task<YnabCategoryData> GetCategories(string accessToken, string budgetId)
        {
            var httpClient = GetHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"budgets/{budgetId}/categories");
            request.Headers.Add("Authorization", "Bearer " + accessToken);

            using (var response = await httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<YnabCategoryData>(data);
            }
        }

        private HttpClient GetHttpClient()
        {
            return _clientFactory.CreateClient("YnabClient");
        }
    }
}
