using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specification
{
    public class ProductSpecificationPrams
    {
        private const int Maxpagesize=10;

        public int PageIndex { get; set; } = 1;

        private int PageSize = 5;

        public int pageSize 
        {
            get { return PageSize = 5;; }
            set { PageSize  = value > Maxpagesize ? Maxpagesize : value; }
        }

        private string Search;

        public string search
        {
            get { return Search; }
            set { Search = value .ToLower(); }
        }

        public string Sort { get; set; }
        public int? Brandid { get; set; }
        public int? Typeid { get; set; }
    }
}
