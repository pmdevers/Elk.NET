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

            Uri = new Uri(url);

            Index = index.ToLower() + "-" + DateTime.UtcNow.ToString("yyyy-MM-dd");

            _client = new ElasticsearchClient(new ConnectionSettings(Uri));
        }

        public override void Write(string message)
        {
            var timeStamp = DateTime.UtcNow.ToString("o");
            var source = Process.GetCurrentProcess().ProcessName;
            var stacktrace = Environment.StackTrace;

            var jObject = new JObject
            {
                { "timestamp", timeStamp },
                { "source", source },
                { "stacktrace", stacktrace },
                { "message", message }
            };

            try
            {
                _client.Index(Index, "Trace", jObject.ToString());
            }
            catch (Exception)
            {
                
            }
            
            //var method = MethodInfo.GetCurrentMethod().
        }

        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }
    }
}
