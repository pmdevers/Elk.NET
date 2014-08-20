using System;
using System.Collections.Generic;
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
                ElkLog.Instance.Debug(new CustomException("Error", ex));
            }
        }


        public static void Test()
        {
            throw new HttpParseException("Not Found!");

            throw new NotImplementedException();
        }
    }
}
