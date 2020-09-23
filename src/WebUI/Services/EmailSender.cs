using Microsoft.AspNetCore.Identity.UI.Services;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rentasgt.WebUI.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailService emailService;

        public EmailSender(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return this.emailService.SendAsync(email, subject, message, true);
        }

    }
}
