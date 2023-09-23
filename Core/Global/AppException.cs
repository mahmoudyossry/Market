using System;
using System.Globalization;

namespace Market.Core.Global
{
    public class AppException : Exception
    {
        public readonly string ErrorNumber;
        public readonly ExceptionEnum errorNumber;
        public string ErrorMessage = "";
        public AppException(string errorMessage) : base(errorMessage) { 
            ErrorMessage = errorMessage;
        }
        public AppException(ExceptionEnum errorNumber,params object?[] args) : base()
        {
            this.errorNumber = errorNumber;

            AppExceptions.ExceptionMessages.TryGetValue((int)this.errorNumber, out this.ErrorMessage);
            try
            {
                this.ErrorMessage = string.Format(this.ErrorMessage, args);
            }
            catch 
            {

            }
        }


    }
}
