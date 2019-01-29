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
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public Ynab(IConfiguration config)
        {
            this._config = config;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_config["Ynab:Url:BaseUrl"]),
                Timeout = new TimeSpan(0, 0, 39),
            };
        }

        public async Task<YnabBudgetData> GetBudgets(string accessToken)
        {
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var response = await _httpClient.GetAsync("budgets");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<YnabBudgetData>(data);
        }
    }
}
