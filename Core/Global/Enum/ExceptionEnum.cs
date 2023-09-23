using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Global
{
    public enum ExceptionEnum
    {
        RecordNotExist = 1,
        ModelNotValid = 2,
        NotAuthorized = 3,
        PropertyNotAccess = 4,
        WrongCredentials = 5,
        RecordCannotBeDelete = 6,
        RecordAlreadyExist = 7,
        RecordCreationFailed = 8,
        RecordUpdateFailed = 9,
        RecordDeleteFailed = 10,
        RecordNameAlreadyExist = 11,
        RecordEmailAlreadyExist = 12,
        TenancyNameAlreadyExist = 13,
        TenantExpected = 14,
        TenantNotZero = 15,
        AttachmentsRequired = 16,
        CouldNotMoveFiles = 17,
        MapperIssue = 18,
        InCorrectFileLength = 19,
        NotAvailableSlot = 20,
        NotAvailableFixedSlot = 21,
    }
}
