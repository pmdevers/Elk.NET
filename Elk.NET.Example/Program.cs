using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Elk.NET.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Test();
            }
            catch (Exception ex)
            {
                Trace.Write("Exception");
                ElkLog.Instance.Debug(ex);
            }
        }


        public static void Test()
        {
            Trace.Write("Entering");

            throw new HttpException(404, "Not Found!");




            throw new HttpParseException("Not Found!");

            Trace.Write("Exiting");
        }
    }
}
