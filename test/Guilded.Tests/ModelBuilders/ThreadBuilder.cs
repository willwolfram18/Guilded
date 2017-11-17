using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guilded.Data.Forums;

namespace Guilded.Tests.ModelBuilders
{
    public class ThreadBuilder : ModelBuilder<Thread>
    {
        public ThreadBuilder()
        {
            Instance = new Thread();
        }

        public void ThreadDoesNotExist()
        {
            Instance = null;
        }

        public ThreadBuilder WithId(int id)
        {
            Instance.Id = id;

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
    }
}
