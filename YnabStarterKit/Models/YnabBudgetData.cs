using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnabStarterKit.Models
{
    public class YnabBudgetData
    {
        public YnabBudgetDataItem Data { get; set; }
    }

    public class YnabBudgetDataItem
    {
        public List<YnabBudget> Budgets { get; set; }
    }

    public class YnabBudget
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
