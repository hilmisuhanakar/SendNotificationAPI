using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SendNotificationAPI.Models.InterfacesModelType;

namespace SendNotificationAPI.Models.RequestModel
{
    public class SendMailNotificationRequest : TNotificationRequestModel
    {
        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; } = null!;

        [Required(ErrorMessage = "Recipients is required")]
        
        public string[] Recipients { get; set; } = null!;
        public string[]? CC { get; set; }
        public string[]? BCC { get; set; }
        public string[]? Attachments { get; set; }
    }
}