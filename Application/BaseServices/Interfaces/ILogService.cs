using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Services.Interfaces
{
    public interface ILogService<TEntity>
    {
        Task Create(TEntity input);
    }
}
