using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Clients;
using Twilio.Http;
using WhatsApp.Messaging.Twilio.Options;

namespace WhatsApp.Messaging.Twilio.Http
{
    public sealed class TwilioClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<TwilioOptions> _opts;

        public TwilioClientFactory(IHttpClientFactory httpClientFactory, IOptions<TwilioOptions> opts)
        {
            _httpClientFactory = httpClientFactory;
            _opts = opts;
        }

        public ITwilioRestClient Create()
        {
            var http = _httpClientFactory.CreateClient("twilio");
            return new TwilioRestClient(
                _opts.Value.AccountSid, 
                _opts.Value.AuthToken, 
                httpClient: new SystemNetHttpClient(http));
        }
    }
}
