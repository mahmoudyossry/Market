using Market.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Entities
{
    public class Product :FullAuditEntity
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
