using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using StoryCLM.SDK.Myosotis.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Myosotis
{
    public class MyosotisCommand<TResult> : BackendCommand where TResult : class
    {
        const string kMediaTypeHeader = "application/json";

        public TResult Result { get; set; }

        public object Data { get; set; }

        public async override Task OnExecuting(HttpRequestMessage request, CancellationToken token)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(kMediaTypeHeader));
            if (Data == null) return;
            request.Content = new StringContent(JsonConvert.SerializeObject(Data, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd" }), Encoding.UTF8, kMediaTypeHeader);
        }

        public async override Task OnExecuted(HttpResponseMessage response, CancellationToken token)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return;
            string result = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode != HttpStatusCode.OK) throw new HttpCommandException((int)response.StatusCode, result);
            MyoBase baseMessage = JsonConvert.DeserializeObject<MyoBase>(result);
            JToken data = baseMessage.Data as JToken;
            if (data == null) return;
            Result = data.ToObject<TResult>();
        }
    }
}
