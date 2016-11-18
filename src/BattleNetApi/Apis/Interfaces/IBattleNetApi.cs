namespace BattleNetApi.Apis.Interfaces
{
    public interface IBattleNetApi
    {
        IOAuthApiMethods OAuthApi { get; }
        IWowCommunityApiMethods WowCommunityApi { get; }
    }
}
