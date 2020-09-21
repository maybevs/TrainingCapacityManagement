using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingCapacityManagement.Services
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }

        internal void UseSendGrid(string sendGridUser, string sendGridKey)
        {
            SendGridUser = sendGridUser;
            SendGridKey = sendGridKey;
        }
    }
}
