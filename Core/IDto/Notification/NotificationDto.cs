using Market.Core.Global.Enum;
using Market.Core.IDto;
using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Core.Dto
{
    public class NotificationDto
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Message cannot be more than 250 charachters")]
        public string Message { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public string? InfoId { get; set; }
        public string? InfoDesc { get; set; }
        public DateTime CreateDate { get; set; }
        [Required]
        public string UserId { get; set; }
        public bool IsRead { get; set; }
       
    }
}
