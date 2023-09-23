using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Entities.Base
{
    public interface ISoftDelete
    {
        [Column(Order = 7)]
        public bool IsDeleted { get; set; }
        [MaxLength(450)]
        [Column(Order = 8)]
        public string DeleteUserId { get; set; }
    }
}
