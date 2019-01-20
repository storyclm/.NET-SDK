using StoryCLM.SDK.IoT.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.IoT
{
    public class Feed : IEnumerable<FeedPage>, IEnumerator<FeedPage>
    {
        const string IOT = "iot";
        string _position;

        public Feed(string continuationToken = null)
        {
            ContinuationToken = continuationToken;
            _position = ContinuationToken;
        }

        public string ContinuationToken { get; set; }

        public CancellationToken CancellationToken { get; set; }

        object IEnumerator.Current => Current;

        public FeedPage Current { get; private set; }

        internal SCLM _sclm { get; set; }

        internal IoTParameters _parameters { get; set; }

        internal Section _section { get; set; }

        IEnumerator IEnumerable.GetEnumerator() => this;

        public IEnumerator<FeedPage> GetEnumerator() => this;

        public bool MoveNext()
        {
            Current = Next().Result;
            if (Current == null || string.IsNullOrEmpty(Current.ContinuationToken))
            {
                Reset();
                return false;
            }
            return true;
        }

        public void Reset() =>
            _position = ContinuationToken;

        public async Task<FeedPage> Next()
        {
            var command = new FeedCommand(_parameters)
            {
                ContinuationToken = _position,
                Endpoint = new Uri($"{_sclm.GetEndpoint(IOT)}{_parameters.Hub}/feed/{_section}")
            };

            if (!string.IsNullOrEmpty(_position)) // кастыль!!
                command.SetParameter("token", _position);

            await _sclm.ExecuteHttpCommand(command, _sclm.HttpQueryPolicy, CancellationToken).ConfigureAwait(false);
            _position = command.ContinuationToken;

            foreach (var t in command.Messages)
            {
                t._parameters = _parameters;
                t._sclm = _sclm;
            }

            return new FeedPage
            {
                ContinuationToken = command.ContinuationToken,
                Messages = command.Messages
            };
        }

        public void Dispose() =>
            Reset();
    }
}
