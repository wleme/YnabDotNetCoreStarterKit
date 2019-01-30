using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnabStarterKit.Models
{
    public class YnabCategoryData
    {
        public YnabCategoryGroupItem Data { get; set; }
    }

    public class YnabCategoryGroupItem
    {
        public IEnumerable<YnabCategoryGroup> Category_Groups { get; set; }
    }

    public class YnabCategoryGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Hidden { get; set; }
        public bool Deleted { get; set; }
        public IEnumerable<YnabCategory> Categories { get; set; }
    }

    public class YnabCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Hidden { get; set; }
    }


}
