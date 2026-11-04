using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendNotificationAPI.Models.Options
{
    public class SMTPOptions
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}