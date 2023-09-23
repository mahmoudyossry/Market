using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Entities.Base
{
    public interface IEntity
    {
        [Column(Order = 0)]
        long Id { get; set; }
    }
    public interface IEntity<TKey>
    {
        [Column(Order = 0)]
        TKey Id { get; set; }
    }
}
