namespace Scarf
{
    public static class ScarfAction
    {
        public static void Start(string messageType)
        {
            ScarfLogging.CurrentContext.CreateMessage(MessageClass.Action, messageType);
        }
        public static void SetMessage(string message)
        {
            ScarfContext.CurrentInternal.PrimaryMessage.Message = message;
        }
    }
}