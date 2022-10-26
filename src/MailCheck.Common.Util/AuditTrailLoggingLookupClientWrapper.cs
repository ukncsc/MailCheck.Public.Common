using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Logging;

namespace MailCheck.Common.Util
{
    public class AuditTrailLoggingLookupClientWrapper : ILookupClient
    {
        private ILookupClient _lookupClient;
        private IAuditTrailParser _auditTrailParser;
        private ILogger<AuditTrailLoggingLookupClientWrapper> _log;

        public AuditTrailLoggingLookupClientWrapper(ILookupClient lookupClient, IAuditTrailParser parser, ILogger<AuditTrailLoggingLookupClientWrapper> logger)
        {
            _lookupClient = lookupClient;
            _auditTrailParser = parser;
            _log = logger;
        }

        IReadOnlyCollection<NameServer> ILookupClient.NameServers => _lookupClient.NameServers;
        LookupClientSettings ILookupClient.Settings => _lookupClient.Settings;

        TimeSpan? ILookupClient.MinimumCacheTimeout { get => _lookupClient.MinimumCacheTimeout; set => _lookupClient.MinimumCacheTimeout = value; }
        bool ILookupClient.EnableAuditTrail { get => _lookupClient.EnableAuditTrail; set => _lookupClient.EnableAuditTrail = value; }
        bool ILookupClient.UseCache { get => _lookupClient.UseCache; set => _lookupClient.UseCache = value; }
        bool ILookupClient.Recursion { get => _lookupClient.Recursion; set => _lookupClient.Recursion = value; }
        int ILookupClient.Retries { get => _lookupClient.Retries; set => _lookupClient.Retries = value; }
        bool ILookupClient.ThrowDnsErrors { get => _lookupClient.ThrowDnsErrors; set => _lookupClient.ThrowDnsErrors = value; }
        bool ILookupClient.UseRandomNameServer { get => _lookupClient.UseRandomNameServer; set => _lookupClient.UseRandomNameServer = value; }
        bool ILookupClient.ContinueOnDnsError { get => _lookupClient.ContinueOnDnsError; set => _lookupClient.ContinueOnDnsError = value; }
        TimeSpan ILookupClient.Timeout { get => _lookupClient.Timeout; set => _lookupClient.Timeout = value; }
        bool ILookupClient.UseTcpFallback { get => _lookupClient.UseTcpFallback; set => _lookupClient.UseTcpFallback = value; }
        bool ILookupClient.UseTcpOnly { get => _lookupClient.UseTcpOnly; set => _lookupClient.UseTcpOnly = value; }

        public IDnsQueryResponse Query(DnsQuestion question)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.Query(question);
            return MakeQuery(QueryFunc);
        }

        public IDnsQueryResponse Query(DnsQuestion question, DnsQueryAndServerOptions queryOptions)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.Query(question, queryOptions);

            return MakeQuery(QueryFunc);
        }

        public IDnsQueryResponse Query(string query, QueryType queryType, QueryClass queryClass = QueryClass.IN)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.Query(query, queryType, queryClass);

            return MakeQuery(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryAsync(string query, QueryType queryType, QueryClass queryClass = QueryClass.IN, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryAsync(query, queryType, queryClass, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryAsync(DnsQuestion question, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryAsync(question, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryAsync(DnsQuestion question, DnsQueryAndServerOptions queryOptions, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryAsync(question, queryOptions, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public IDnsQueryResponse QueryReverse(IPAddress ipAddress)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.QueryReverse(ipAddress);

            return MakeQuery(QueryFunc);
        }

        public IDnsQueryResponse QueryReverse(IPAddress ipAddress, DnsQueryAndServerOptions queryOptions)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.QueryReverse(ipAddress, queryOptions);

            return MakeQuery(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryReverseAsync(IPAddress ipAddress, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryReverseAsync(ipAddress, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryReverseAsync(IPAddress ipAddress, DnsQueryAndServerOptions queryOptions, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryReverseAsync(ipAddress, queryOptions, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public IDnsQueryResponse QueryServer(IReadOnlyCollection<NameServer> servers, string query, QueryType queryType, QueryClass queryClass = QueryClass.IN)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.QueryServer(servers, query, queryType, queryClass);

            return MakeQuery(QueryFunc);
        }

        public IDnsQueryResponse QueryServer(IReadOnlyCollection<NameServer> servers, DnsQuestion question)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.QueryServer(servers, question);

            return MakeQuery(QueryFunc);
        }

        public IDnsQueryResponse QueryServer(IReadOnlyCollection<NameServer> servers, DnsQuestion question, DnsQueryOptions queryOptions)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.QueryServer(servers, question, queryOptions);

            return MakeQuery(QueryFunc);
        }

        public IDnsQueryResponse QueryServer(IReadOnlyCollection<IPEndPoint> servers, string query, QueryType queryType, QueryClass queryClass = QueryClass.IN)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.QueryServer(servers, query, queryType, queryClass);

            return MakeQuery(QueryFunc);
        }

        public IDnsQueryResponse QueryServer(IReadOnlyCollection<IPAddress> servers, string query, QueryType queryType, QueryClass queryClass = QueryClass.IN)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.QueryServer(servers, query, queryType, queryClass);

            return MakeQuery(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryServerAsync(IReadOnlyCollection<NameServer> servers, string query, QueryType queryType, QueryClass queryClass = QueryClass.IN, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryServerAsync(servers, query, queryType, queryClass, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryServerAsync(IReadOnlyCollection<NameServer> servers, DnsQuestion question, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryServerAsync(servers, question, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryServerAsync(IReadOnlyCollection<NameServer> servers, DnsQuestion question, DnsQueryOptions queryOptions, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryServerAsync(servers, question, queryOptions, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryServerAsync(IReadOnlyCollection<IPAddress> servers, string query, QueryType queryType, QueryClass queryClass = QueryClass.IN, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryServerAsync(servers, query, queryType, queryClass, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryServerAsync(IReadOnlyCollection<IPEndPoint> servers, string query, QueryType queryType, QueryClass queryClass = QueryClass.IN, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryServerAsync(servers, query, queryType, queryClass, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public IDnsQueryResponse QueryServerReverse(IReadOnlyCollection<IPAddress> servers, IPAddress ipAddress)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.QueryServerReverse(servers, ipAddress);

            return MakeQuery(QueryFunc);
        }

        public IDnsQueryResponse QueryServerReverse(IReadOnlyCollection<IPEndPoint> servers, IPAddress ipAddress)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.QueryServerReverse(servers, ipAddress);

            return MakeQuery(QueryFunc);
        }

        public IDnsQueryResponse QueryServerReverse(IReadOnlyCollection<NameServer> servers, IPAddress ipAddress)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.QueryServerReverse(servers, ipAddress);

            return MakeQuery(QueryFunc);
        }

        public IDnsQueryResponse QueryServerReverse(IReadOnlyCollection<NameServer> servers, IPAddress ipAddress, DnsQueryOptions queryOptions)
        {
            IDnsQueryResponse QueryFunc() => _lookupClient.QueryServerReverse(servers, ipAddress, queryOptions);

            return MakeQuery(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryServerReverseAsync(IReadOnlyCollection<IPAddress> servers, IPAddress ipAddress, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryServerReverseAsync(servers, ipAddress, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryServerReverseAsync(IReadOnlyCollection<IPEndPoint> servers, IPAddress ipAddress, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryServerReverseAsync(servers, ipAddress, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryServerReverseAsync(IReadOnlyCollection<NameServer> servers, IPAddress ipAddress, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryServerReverseAsync(servers, ipAddress, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        public Task<IDnsQueryResponse> QueryServerReverseAsync(IReadOnlyCollection<NameServer> servers, IPAddress ipAddress, DnsQueryOptions queryOptions, CancellationToken cancellationToken = default)
        {
            Task<IDnsQueryResponse> QueryFunc() => _lookupClient.QueryServerReverseAsync(servers, ipAddress, queryOptions, cancellationToken);

            return MakeQueryAsync(QueryFunc);
        }

        private IDnsQueryResponse MakeQuery(Func<IDnsQueryResponse> func)
        {
            try
            {
                IDnsQueryResponse response = func();
                RecordAudit(response.AuditTrail);
                return response;
            }
            catch(DnsResponseException ex)
            {
                RecordAudit(ex.AuditTrail);
                throw;
            }
        }

        private async Task<IDnsQueryResponse> MakeQueryAsync(Func<Task<IDnsQueryResponse>> func)
        {
            try
            {
                IDnsQueryResponse response = await func();
                RecordAudit(response.AuditTrail);
                return response;
            }
            catch (DnsResponseException ex)
            {
                RecordAudit(ex.AuditTrail);
                throw;
            }
        }

        private void RecordAudit(string audit)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(audit)) return;

                List<AuditParseResult> auditResults = _auditTrailParser.Parse(audit);

                foreach (AuditParseResult result in auditResults)
                {
                    _log.LogInformation($"DNS Query made to: {result.NameServer}, Message Size: {result.MessageSize}, Error: {result.Error}, Query Time: {result.QueryTime}");
                }
            }
            catch(Exception e)
            {
                _log.LogError($"Error when parsing audit trail: {e.Message} {Environment.NewLine} {e.StackTrace}");
            }
        }
    }
}