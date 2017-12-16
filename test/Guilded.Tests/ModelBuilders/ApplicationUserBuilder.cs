using Guilded.Data.Identity;

namespace Guilded.Tests.ModelBuilders
{
    public class ApplicationUserBuilder : ModelBuilder<ApplicationUser>
    {
        public ApplicationUserBuilder()
        {
            Instance = new ApplicationUser();
        }

        public ApplicationUserBuilder WithId(string userId)
        {
            Instance.Id = userId;

            return this;
        }

        public ApplicationUserBuilder WithUserName(string userName)
        {
            Instance.UserName = userName;

            return this;
        }
    }
}
