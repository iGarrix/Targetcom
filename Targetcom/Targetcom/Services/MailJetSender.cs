using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Services
{
    public class MailJetSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public MailJetKeys _mailJetKeys { get; set; }

        public MailJetSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            _mailJetKeys = _configuration.GetSection("MailJet").Get<MailJetKeys>();

            MailjetClient client = new MailjetClient(_mailJetKeys.ApiKey, _mailJetKeys.SecretKey)
            {
                Version = ApiVersion.V3_1
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.Messages, new JArray {
             new JObject {
              {
               "From",
               new JObject {
                {"Email", Env.Gmail},
                {"Name", "Target.com"}
               }
              }, {
               "To",
               new JArray {
                new JObject {
                 {
                  "Email",
                  email
                 }, {
                  "Name",
                  "Target.com"
                 }
                }
               }
              }, {
               "Subject",
               subject
              },{
               "HTMLPart",
              body
              }
             }
            });
            await client.PostAsync(request);
        }
    }
}
