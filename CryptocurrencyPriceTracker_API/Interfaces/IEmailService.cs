using CryptocurrencyPriceTracker_API.Model.User;

namespace CryptocurrencyPriceTracker_API.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(EmailModel emailModel);
    }
}
