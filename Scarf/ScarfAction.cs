﻿using System;

namespace Scarf.MVC
{
    public class ScarfAction
    {
        public static void SetMessage(string message)
        {
            ScarfContext.Current.CurrentMessage.Message = message;
        }
    }
}