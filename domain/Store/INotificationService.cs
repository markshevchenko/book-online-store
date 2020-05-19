namespace Store
{
    public interface INotificationService
    {
        void SendConfirmationCode(string cellPhone, int code);
    }
}
