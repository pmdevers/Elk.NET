using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elk.NET.Example
{
    public class CustomException : Exception
    {
        public CustomException(string message, Exception ex)
            : base(message, ex)
        {
            IpAddress = "127.0.0.1";
            Environment = "LIVE";
        }
        public string IpAddress { get; private set; }

        public string Environment { get; set; }
    }
}
