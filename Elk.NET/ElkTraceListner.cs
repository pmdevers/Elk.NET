using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Elasticsearch.Net;

using Nest;

using Newtonsoft.Json.Linq;

namespace Elk.NET
{
    public class ElkTraceListner : TraceListener
    {
        private const string ElasticSearchUri = "ElasticSearchUri";
        private const string ElasticSearchTraceIndex = "ElasticSearchTraceIndex";

        private readonly ElasticsearchClient _client;

        public Uri Uri { get; private set; }
        public string Index { get; private set; }
        public ElkTraceListner()
        {
            var url = ConfigurationManager.AppSettings.Get(ElasticSearchUri);
            var index = ConfigurationManager.AppSettings.Get(ElasticSearchTraceIndex);

            if (string.IsNullOrEmpty(url))
                throw new ApplicationException(string.Format("Elk.NET setting {0} not set.", ElasticSearchUri));
            if (string.IsNullOrEmpty(index))
                throw new ApplicationException(string.Format("Elk.NET setting {0} not set.", ElasticSearchTraceIndex));

            Uri = new Uri(url);

            Index = index.ToLower() + "-" + DateTime.UtcNow.ToString("yyyy-MM-dd");

            _client = new ElasticsearchClient(new ConnectionSettings(Uri));
        }

        public override void Write(string message)
        {
            SendMethod(message);
        }

        public override void WriteLine(string message)
        {
            SendMethod(message);
        }

        private void SendMethod(string message)
        {
            var timeStamp = DateTime.UtcNow.ToString("o");
            var source = Process.GetCurrentProcess().ProcessName;
            var stacktrace = Environment.StackTrace;
            var methodName = (new StackTrace()).GetFrame(StackTrace.METHODS_TO_SKIP + 4).GetMethod().Name;

            var jObject = new JObject
            {
                { "timestamp", timeStamp },
                { "source", source },
                { "stacktrace", stacktrace },
                { "message", message },
                { "method", methodName }
            };

            try
            {
                _client.Index(Index, "Trace", jObject.ToString());
            }
            catch (Exception)
            {
                
            }
        }
    }
}
