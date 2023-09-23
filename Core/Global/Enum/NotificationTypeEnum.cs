using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Global.Enum
{
    public enum NotificationTypeEnum :int
    {
        NewClientRegistered = 1,
        NewClientAssigned,
        ClientTypeloss,
        ClientTypeReview,
        NewSalesRequestCreated,
        NewSalesRequestAssigned,
        NewSalesRequestToAssign,
        SalesRequestAssignedToQuotation
    }
}
