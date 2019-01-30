using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnabStarterKit.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly Services.Ynab _ynab;
        
        public BudgetsController(Services.Ynab ynab)
        {
            this._ynab = ynab;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var data = await _ynab.GetBudgets(token);
            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Categories(string budgetId)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var data = await _ynab.GetCategories(token,budgetId);
            return View(data);
        }
    }
}
