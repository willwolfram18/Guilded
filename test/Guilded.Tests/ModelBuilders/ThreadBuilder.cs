using Guilded.Data.Forums;
using System.Collections.Generic;

namespace Guilded.Tests.ModelBuilders
{
    public class ThreadBuilder : ModelBuilder<Thread>
    {
        private readonly ApplicationUserBuilder _userBuilder;

        public ThreadBuilder()
        {
            Instance = new Thread();

            _userBuilder = new ApplicationUserBuilder();
        }

        public void DoesNotExist()
        {
            Instance = null;
        }

        public ThreadBuilder WithId(int id)
        {
            Instance.Id = id;

            return this;
        }

        public ThreadBuilder WithSlug(string slug)
        {
            Instance.Slug = slug;

            return this;
        }

        public ThreadBuilder WithAuthorId(string authorId)
        {
            _userBuilder.WithId(authorId);

            return this;
        }

        public ThreadBuilder WithAuthorName(string authorName)
        {
            _userBuilder.WithUserName(authorName);

            return this;
        }

        public ThreadBuilder WithNoReplies()
        {
            Instance.Replies = new List<Reply>();

            return this;
        }

        public ThreadBuilder IsDeleted()
        {
            Instance.IsDeleted = true;

            return this;
        }

        public ThreadBuilder IsLocked()
        {
            Instance.IsLocked = true;

            return this;
        }

        public ThreadBuilder IsUnlocked()
        {
            Instance.IsLocked = false;

            return this;
        }

        public ThreadBuilder WithActiveForum()
        {
            Instance.Forum = new Forum
            {
                IsActive = true
            };

            return this;
        }

        public ThreadBuilder WithInactiveForum()
        {
            Instance.Forum = new Forum
            {
                IsActive = false
            };

            return this;
        }

        public ThreadBuilder IsPinned()
        {
            Instance.IsPinned = true;

            return this;
        }

        public ThreadBuilder IsUnpinned()
        {
            Instance.IsPinned = false;

            return this;
        }

        protected override void BeforeBuild()
        {
            if (Instance == null)
            {
                return;
            }

            Instance.Author = _userBuilder.Build();
            Instance.AuthorId = Instance.Author.Id;
        }
    }
}
