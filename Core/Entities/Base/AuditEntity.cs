using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Entities.Base
{
    public class AuditEntity:Entity,IAuditEntity
    {
        [MaxLength(450)]
        [Column(Order = 1)]
        public string? CreateUser { get; set; }
        [MaxLength(256)]
        [Column(Order = 2)]
        public string CreateUserName { get; set; }
        [MaxLength(450)]
        [Column(Order = 3)]
        public string? UpdateUser { get; set; }
        [MaxLength(256)]
        [Column(Order = 4)]
        public string UpdateUserName { get; set; }
        [Column(Order = 5)]
        public DateTime CreateDate { get; set; }
        [Column(Order = 6)]
        public DateTime? UpdateDate { get; set; }
    }

    public class AuditEntity<TKey> : Entity<TKey>, IAuditEntity<TKey> where TKey : Type
    {
        [MaxLength(450)]
        [Column(Order = 1)]
        public string? CreateUser { get; set; }
        [MaxLength(256)]
        [Column(Order = 2)]
        public string CreateUserName { get; set; }
        [MaxLength(450)]
        [Column(Order = 3)]
        public string? UpdateUser { get; set; }
        [MaxLength(256)]
        [Column(Order = 4)]
        public string UpdateUserName { get; set; }
        [Column(Order = 5)]
        public DateTime CreateDate { get; set; }
        [Column(Order = 6)]
        public DateTime? UpdateDate { get; set; }
    }
}
