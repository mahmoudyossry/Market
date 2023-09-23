
using Market.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Market.Application.Dto
{
    public class ProductDto : EntityDto
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public DateTime ExpiryDate { get; set; }= DateTime.Now.AddYears(2);
    }
}
