Elk.NET
=======

A simple Kibana exception logger for use with ElasticSearch and Kibana

##Getting Started

Install the package from nu-get http://www.nuget.org/packages/Elk.NET/

And add the following settings to your application

    <add key="ElasticSearchUri" value="http://127.0.0.1:9200" />
    <add key="ElasticSearchIndex" value="elknet" />

***Please note: Elk.NET automaticly appends the date to the index***
the kibana format of the index is [elknet-]YYYY-MM-DD

##Usage

Now that Elk.NET is configured you can simply use it like

    try
    {
        MethodThrowingError();
    }
    catch (Exception ex)
    {
        ElkLog.Instance.Debug(ex);
    }

This will log the error to Kibana.

##Kibana

Open the your Kibana GUI en use the following config file:

[kibana.json][1]


  [1]: https://raw.githubusercontent.com/pmdevers/Elk.NET/master/Elk.NET.Example/kibana.json
