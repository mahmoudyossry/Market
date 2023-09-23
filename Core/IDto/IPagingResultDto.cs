using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.IDto
{
    public interface IPagingResultDto<TEntity> where TEntity : class
    {
        int Total { get; set; }

        IList<TEntity>? Result { get; set; }

    }
}
