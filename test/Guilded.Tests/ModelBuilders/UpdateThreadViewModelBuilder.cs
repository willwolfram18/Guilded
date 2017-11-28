using Guilded.Areas.Forums.ViewModels;

namespace Guilded.Tests.ModelBuilders
{
    public class UpdateThreadViewModelBuilder : ModelBuilder<UpdateThreadViewModel>
    {
        public UpdateThreadViewModelBuilder()
        {
            Instance = new UpdateThreadViewModel();
        }

        public UpdateThreadViewModelBuilder WithThreadId(int id)
        {
            Instance.ThreadId = id;

            return this;
        }

        public UpdateThreadViewModelBuilder WithContent(string content)
        {
            Instance.UpdatedContent = content;

            return this;
        }
    }
}
