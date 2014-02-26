using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log3a.Configuration;

namespace ConfigurationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ScarfSection log3a = ConfigurationManager.GetSection("log3a") as ScarfSection;

            try
            {
                MyAssert(log3a != null);
                MyAssert(log3a.DataAccess != null);
                MyAssert(log3a.DataAccess.ConnectionStringName != null);
                MyAssert(log3a.DataAccess.ConnectionStringName == "myConnection");
                MyAssert(log3a.DataAccess.Type != null);
                MyAssert(log3a.DataAccess.Type == "log3a.DataAccess.SQLServer.SQLServerDataAccess, log3a.SQLServer");

                Console.WriteLine("OK!");
            }
            catch
            {
                Console.WriteLine("NOT OK!!");
            }
        }

        [Conditional("DEBUG")]
        private static void MyAssert(bool expressionThatNeedsToBeTrue)
        {
            if (!expressionThatNeedsToBeTrue)
            {
                throw new InvalidOperationException();
            }

            Console.WriteLine("OK!");
        }
    }
}
