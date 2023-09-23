using Market.Core.IDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Dto
{
    public class PagingResultDto<TEntity>: IPagingResultDto<TEntity> where TEntity : class
    {
        public int Total { get; set; }

        public IList<TEntity>? Result { get; set; }

    }

    public class PagingNotificationResultDto<TEntity> : IPagingResultDto<TEntity> where TEntity : class
    {
        public int Total { get; set; }

        public int TotalUnRead { get; set; }
        public IList<TEntity>? Result { get; set; }

    }

    public class PagingInputDto : IPagingInputDto
    {
        private int pageNumber;
        private int pageSize;
        private string? orderByField;
        private string? orderType;

        public int PageNumber { get => pageNumber ==0 ? 1 : pageNumber; set => pageNumber = value; }
        public int PageSize { get => pageSize==0?10: pageSize; set => pageSize = value; }
        public string? OrderByField { get => string.IsNullOrEmpty(orderByField)?"Id": orderByField; set => orderByField = value; }
        public string? OrderType { get => string.IsNullOrEmpty(orderType) ? "asc" : orderType; set => orderType = value; }
        public string? Filter { get; set; }
        public string HiddenFilter { get ; set; }
    }
}

