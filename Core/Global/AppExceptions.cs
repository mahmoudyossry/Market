using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Global
{
    public static class AppExceptions
    {
        public static IDictionary<int, string> ExceptionMessages = new Dictionary<int, string>()
        {
            {1, "Record Not Existed"},
            {2, "Model is not valid"},
            {3, "Not Authorized" },
            {4, "A property called {0} can't be accessed for type {1}." },
            {5, "Username or password incorrect!"},
            {6, "Cannot delete the user" },
            {7, "Record is already existed!" },
            {8, "Record creation failed! Please check the record details and try again." },
            {9, "Record update failed! Please check the record details and try again." },
            {10, "Record delete failed!" },
            {11, "Record name already existed" },
            {12, "Record email already existed" },
            {13, "Tenancy name already existed" },
            {14, "Tenant expected!" },
            {15, "Tenant cannot be zero!" },
            {16, "Attachments Required" },
            {17, "Could Not Move Files" },
            {18, "Mapper Issue" },
            {19, "InCorrect File Length" },
            {20, "Slot Not Available" },
            {21, "Slot Not Available for fixed, check without fixed" },
        };
    }
}
