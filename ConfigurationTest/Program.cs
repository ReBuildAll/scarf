using System;
using System.Configuration;
using System.Diagnostics;
using Scarf.Configuration;

namespace ConfigurationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ScarfSection scarfSection = ConfigurationManager.GetSection("scarf") as ScarfSection;

            try
            {
                MyAssert(scarfSection != null);
                MyAssert(scarfSection.DataSource != null);
                MyAssert(scarfSection.DataSource.ConnectionStringName != null);
                MyAssert(scarfSection.DataSource.ConnectionStringName == "myConnection");
                MyAssert(scarfSection.DataSource.Type != null);
                MyAssert(scarfSection.DataSource.Type == "log3a.DataSource.SQLServer.SQLServerDataAccess, log3a.SQLServer");

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
        }
    }
}
