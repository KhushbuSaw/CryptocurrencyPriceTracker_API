using System.Data;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace CryptocurrencyPriceTracker_API.Helper
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
    <head>
    </head>
    <body style=""margin:0;PaddingMode:0;font-family:Arial,Helvetica,sans-serif;"">
        <div style=""heigth:auto;background:linear-gradient(to top,#c9c9ff 50%,#6e6ef6 90%)no-repeat;width:400px;padding:30px"">
            <div>
                <div>
                    <h1>Reset Your Password</h1>
                    <hr>
                    <p>Yor're receiving this email because you requested a password reset for your Cryptocurrency Price Tracker Account.</p>
                    <p>Please tab the button below to reset your password.</p>
                    <a href=""http://localhost:4200/reset?email={email}&code={emailToken}"" target=""_blank"" style=""background:#0d6efd;padding:10px;border:none>
                     color:white;border-radius:4px;display:block;margin:0 auto;width:50%;text-align:center;text-decoration:none"">Reset Password</a><br>
                    <p>Kind Regards,<br><br>
                    Khushbu Saw</p>
                 </div>
              </div>
        </div>
    </body>
    </html>";
        }
    }
}
