using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendNotificationAPI.Models.InterfacesModelType;

namespace SendNotificationAPI.Models.RequestModel
{
    public class SendMailNotificationRequest : TNotificationRequestModel
    {
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string[] Recipients { get; set; } = null!;
        public string[]? CC { get; set; }
        public string[]? BCC { get; set; }
        public string[]? Attachments { get; set; }
    }
}