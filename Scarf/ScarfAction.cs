namespace Scarf
{
    public static class ScarfAction
    {
        public static void Start(string messageType)
        {
            ScarfContext.Current.CreatePrimaryMessage(MessageClass.Action, messageType);
        }
        public static void SetMessage(string message)
        {
            ScarfContext.Current.PrimaryMessage.Message = message;
        }
    }
}