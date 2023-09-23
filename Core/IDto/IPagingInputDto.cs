using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.IDto
{
    public interface IPagingInputDto
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string OrderByField { get; set; }
        string OrderType { get; set; }
        string Filter { get; set; }
        public string HiddenFilter { get; set; }
    }
}
