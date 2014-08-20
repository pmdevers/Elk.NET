using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Elasticsearch.Net;

using Nest;

namespace Elk.NET
{
    public class ElkTraceListner : TraceListener
    {
        private const string ElasticSearchUri = "ElasticSearchUri";
        private const string ElasticSearchTraceIndex = "ElasticSearchIndex";

        private readonly ElasticsearchClient _client;

        public Uri Uri { get; private set; }
        public string Index { get; private set; }
        public ElkTraceListner()
        {
            var url = ConfigurationManager.AppSettings.Get(ElasticSearchUri);
            var index = ConfigurationManager.AppSettings.Get(ElasticSearchTraceIndex);

            Uri = new Uri(url);

            Index = index.ToLower() + "-" + DateTime.UtcNow.ToString("YYYY-MM-DD");

            _client = new ElasticsearchClient(new ConnectionSettings());
        }

        public override void Write(string message)
        {
            var timeStamp = DateTime.UtcNow.ToString("o");
            var appName = AppDomain.CurrentDomain.FriendlyName;
        }

        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }
    }
}
