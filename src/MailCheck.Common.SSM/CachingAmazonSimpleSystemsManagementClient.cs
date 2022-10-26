using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using MailCheck.Common.Util;
using MailCheck.Common.Util.Cache;

namespace MailCheck.Common.SSM
{
    public class CachingAmazonSimpleSystemsManagementClient : AmazonSimpleSystemsManagementClient
    {
        private readonly ICache<GetParameterResponse> _cache = new NaiveCache<GetParameterResponse>(new Clock());

        public override Task<GetParameterResponse> GetParameterAsync
            (GetParameterRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            return _cache.GetOrAddAsync(request.Name, () => base.GetParameterAsync(request, cancellationToken), TimeSpan.FromSeconds(60));
        }
    }
}
