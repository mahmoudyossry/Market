using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Entities.Base
{
    public interface IFullAuditEntity : IAuditEntity, IFullAudit
    {

    }

    public interface IFullAuditEntity<TKey> : IAuditEntity<TKey>, IFullAudit
    {

    }

    public interface IFullAudit : ISoftDelete, IAudit
    {
    }
}
