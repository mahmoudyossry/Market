using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Entities.Base
{
    public interface IAuditEntity:IEntity, IAudit
    {

    }

    public interface IAuditEntity<TKey> : IEntity<TKey>, IAudit
    {

    }

    public interface IAudit
    {
        [Column(Order = 1)]
        string? CreateUser { get; set; }
        [Column(Order = 2)]
        string CreateUserName { get; set; }
        [Column(Order = 3)]
        string? UpdateUser { get; set; }
        [Column(Order = 4)]
        string UpdateUserName { get; set; }
        [Column(Order = 5)]
        DateTime CreateDate { get; set; }
        [Column(Order = 6)]
        DateTime? UpdateDate { get; set; }
    }
 }
