using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace VibExchange.Areas.Helper
{
    public class RemoteAnalysisDataRestClient 
    {
        public readonly RestClient _client;
        public readonly string _url = ConfigurationManager.AppSettings["Remote_Analaysis"];

        public RemoteAnalysisDataRestClient()
        {
            _client = new RestClient(_url);
        }
    }
}