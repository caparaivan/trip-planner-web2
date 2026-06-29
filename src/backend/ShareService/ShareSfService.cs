using System.Fabric;
using System.Security.Cryptography;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TripPlanner.Contracts.Dtos;
using TripPlanner.Contracts.Services;

namespace ShareService;

internal sealed class ShareSfService : StatefulService, IShareService
{
    private const string ShareTokensDictionaryName = "shareTokens";

    public ShareSfService(StatefulServiceContext context) : base(context)
    {
    }

    public async Task<ShareTokenDto> CreateShareTokenAsync(ShareTokenCreateDto request)
    {
        var dictionary = await StateManager.GetOrAddAsync<IReliableDictionary<string, ShareTokenDto>>(ShareTokensDictionaryName);
        var shareToken = new ShareTokenDto
        {
            Token = CreateToken(),
            TripPlanId = request.TripPlanId,
            AccessType = request.AccessType,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = request.ExpiresAtUtc
        };

        using var tx = StateManager.CreateTransaction();
        await dictionary.AddOrUpdateAsync(tx, shareToken.Token, shareToken, (_, _) => shareToken);
        await tx.CommitAsync();

        return shareToken;
    }

    public async Task<ShareTokenDto?> GetShareTokenAsync(string token)
    {
        var dictionary = await StateManager.GetOrAddAsync<IReliableDictionary<string, ShareTokenDto>>(ShareTokensDictionaryName);

        using var tx = StateManager.CreateTransaction();
        var result = await dictionary.TryGetValueAsync(tx, token);
        return result.HasValue ? result.Value : null;
    }

    protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
    {
        return this.CreateServiceRemotingReplicaListeners();
    }

    private static string CreateToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}
