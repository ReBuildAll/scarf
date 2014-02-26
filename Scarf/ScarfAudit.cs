using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarf
{
    public static class ScarfAudit
    {
        public static void LoggedInAs(string username)
        {
            ScarfContext.Current.CurrentMessage.Message = string.Format("User '{0}' logged in", username);
            Succeeded();
        }

        public static void CreatedUser(string username)
        {
            ScarfContext.Current.CurrentMessage.Message = string.Format("Created new user '{0}'", username);
            Succeeded();
        }

        public static void Failed()
        {
            ScarfContext.Current.CurrentMessage.Success = false;            
        }
        public static void Succeeded()
        {
            ScarfContext.Current.CurrentMessage.Success = true;
        }
    }
}
