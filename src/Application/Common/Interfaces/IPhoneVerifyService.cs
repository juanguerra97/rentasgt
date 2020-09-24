using System.Threading.Tasks;

namespace rentasgt.Application.Common.Interfaces
{
    public interface IPhoneVerifyService
    {

        Task<bool> IsValidPhoneNumber(string phoneNumber);
        Task<string> SendVerificationCode(string phoneNumber);
        Task<bool> VerifyCode(string phoneNumber, string verificationCode);

    }
}
