﻿using System;
using System.Configuration;

using Elasticsearch.Net;
using Elasticsearch.Net.Connection;

namespace Elk.NET
{
    public class ElkLog
    {
        private const string ElasticSearchUri = "ElasticSearchUri";
        private const string ElasticSearchIndex = "ElasticSearchIndex";

        private readonly ElasticsearchClient _client;
        public string Index { get; private set; }
        public Uri Uri { get; private set; }

        public IExceptionSerializer Serializer { get; private set; }
        
        #region Singleton
        private static readonly Lazy<ElkLog> lazy = new Lazy<ElkLog>(() => new ElkLog());
        public static ElkLog Instance { get { return lazy.Value; } }
        
        private ElkLog()
        {
            var elasticSearch = ConfigurationManager.AppSettings.Get(ElasticSearchUri);
            var index = ConfigurationManager.AppSettings.Get(ElasticSearchIndex);
            Serializer = new ExceptionSerializer();

            if(string.IsNullOrEmpty(elasticSearch))
                throw new ApplicationException(string.Format("Elk.NET setting {0} not set.", ElasticSearchUri));
            if(string.IsNullOrEmpty(index))
                throw new ApplicationException(string.Format("Elk.NET setting {0} not set.", ElasticSearchIndex));

            Uri = new Uri(elasticSearch);
            Index = index.ToLower() + "-" +  DateTime.UtcNow.ToString("yyyy-MM-dd");

            _client = new ElasticsearchClient(new ConnectionConfiguration(Uri));
        }
        #endregion
        
        public void Debug(Exception exception)
        {
            var logDate = DateTime.UtcNow;
            var group = Guid.NewGuid();
            while (exception != null)
            {
                var value = Serializer.Serialize(exception);
                
                value.Add("timestamp", logDate.ToString("o"));
                value.Add("group", group);
                
                var response = _client.Index(Index, exception.GetType().Name, value.ToString());

                exception = exception.InnerException;
            }

            //var test = _client.CountGet(Index);
        }

        public void SetSerializer(IExceptionSerializer serializer)
        {
            this.Serializer = serializer;
        }

        public void ClearLog()
        {
            _client.IndicesDelete(Index);
        }
    }
}
