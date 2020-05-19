using Microsoft.Extensions.Logging;
using Store;
using System.Diagnostics;

namespace Web
{
    class NotificationService : INotificationService
    {
        public void SendConfirmationCode(string cellPhone, int code)
        {
            Debug.WriteLine("Cell phone: '{0}', code: {1:000#}.", cellPhone, code);
        }
    }
}
