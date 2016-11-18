namespace BattleNetApi.Apis.Interfaces
{
    public interface IBattleNetApiClient
    {
        IOAuthApiMethods OAuthApi { get; }
        IWowCommunityApiMethods WowCommunityApi { get; }
    }
}
