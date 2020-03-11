using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace StoryCLM.SDK.CLMAnalitycs
{
    public class CLMAnalitycsFeed<T> : IEnumerable<CLMAnalitycsFeedPage<T>>, IEnumerator<CLMAnalitycsFeedPage<T>> where T : IFeedable
    {
        long? _position;
        readonly string _name;
        readonly int _presentationId;
        readonly string _userId;
        readonly Section _section;

        public CLMAnalitycsFeed(string name,
            int presentationId,
            string userId = null,
            Section section = null, 
            long? continuationToken = null)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _presentationId = presentationId;
            _userId = userId;
            _section = section;
            ContinuationToken = continuationToken;
            _position = ContinuationToken;
        }

        public long? ContinuationToken { get; set; }

        public CancellationToken CancellationToken { get; set; }

        object IEnumerator.Current => Current;

        public CLMAnalitycsFeedPage<T> Current { get; private set; }

        internal SCLM _sclm { get; set; }


        NameValueCollection _parameters = HttpUtility.ParseQueryString(string.Empty);

        IEnumerator IEnumerable.GetEnumerator() => this;

        public IEnumerator<CLMAnalitycsFeedPage<T>> GetEnumerator() => this;

        public bool MoveNext()
        {
            Current = Next().Result;
            if (Current.Result.Count() == 0)
            {
                Reset();
                return false;
            }
            return true;
        }

        public void Reset() => _position = ContinuationToken;

        Uri GetUri(Uri endpoint)
        {
            var builder = new UriBuilder(endpoint);
            _parameters["presentationId"] = _presentationId.ToString();

            if (!string.IsNullOrEmpty(_userId))
                _parameters["userId"] = _userId;

            if (_position != null)
                _parameters["continuationToken"] = _position.ToString();

            builder.Query = _parameters.ToString();
            return builder.Uri;
        }

        public async Task<CLMAnalitycsFeedPage<T>> Next()
        {
            Uri uri = GetUri(new Uri($"{_sclm.GetEndpoint("api")}{AnalitycsExtentions.Version}/{AnalitycsExtentions.Path}/{_name}/{_section}", UriKind.Absolute));
            IEnumerable<T> result = await _sclm.GETAsync<IEnumerable<T>>(uri, CancellationToken.None);
            _position = result.OrderBy(t => t.Ticks).LastOrDefault()?.Ticks;
            return new CLMAnalitycsFeedPage<T>
            {
                ContinuationToken = _position,
                Result = result
            };
        }

        public void Dispose() => Reset();
    }
}
