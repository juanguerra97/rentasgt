
using Microsoft.Extensions.Options;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Infrastructure.Models;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twilio.Rest.Lookups.V1;
using Twilio.Rest.Verify.V2.Service;
using Twilio.Types;

namespace rentasgt.Infrastructure.Services
{
    public class PhoneVerifyService : IPhoneVerifyService
    {

        public static readonly Regex VALID_PHONE_REGEX = new Regex("^[0-9]{8}$", RegexOptions.Compiled);

        private readonly TwilioVerifySettings twilioVerifySettings;

        public PhoneVerifyService(IOptions<TwilioVerifySettings> options)
        {
            this.twilioVerifySettings = options.Value;
        }

        public async Task<bool> IsValidPhoneNumber(string number)
        {
            if (!VALID_PHONE_REGEX.IsMatch(number))
            {
                return false;
            }
            try
            {
                var res = await PhoneNumberResource.FetchAsync(new PhoneNumber("+50259805849"));
                return true;
            } catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<string> SendVerificationCode(string phoneNumber)
        {
            try
            {
                var verification = await VerificationResource.CreateAsync(
                   to: phoneNumber,
                   channel: "sms",
                   pathServiceSid: this.twilioVerifySettings.VerificationServiceSID
               );
                return verification.Status;
            } catch(Exception)
            {
                return "Exception";
            }
        }

        public async Task<bool> VerifyCode(string phoneNumber, string verificationCode)
        {
            try
            {
                var verification = await VerificationCheckResource.CreateAsync(
                    to: phoneNumber,
                    code: verificationCode,
                    pathServiceSid: this.twilioVerifySettings.VerificationServiceSID
                );
                return verification.Status == "approved";
            } catch (Exception)
            {
                return false;
            }
                
        }
    }
}
