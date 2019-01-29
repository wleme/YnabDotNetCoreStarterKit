using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnabStarterKit.Controllers
{
    [Route("/[controller]")]
    public class BudgetsController : Controller
    {
        private readonly Services.Ynab _ynab;
        
        public BudgetsController(Services.Ynab ynab)
        {
            this._ynab = ynab;
        }

        [Route("index")]
        [Authorize]
        public async Task<IActionResult> Budgets()
        {
            var b = await _ynab.GetBudgets("asdf");
            //return View(new ViewModels.AccountInfo() { YnabId = User.FindFirst(c => c.Type == ClaimTypes.Sid)?.Value });
            return View(b);
        }
        [Route("{budgetId:string}")]
        [Authorize]
        public async Task<IActionResult> Budget(string budgetId)
        {
            var b = await _ynab.GetBudgets("asdf");
            return View(b);
        }
    }
}
