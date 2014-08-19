using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ElkLog.Instance.Debug(ex);
            }
        }


        public static void Test()
        {
            throw new NotImplementedException();
        }
    }
}
