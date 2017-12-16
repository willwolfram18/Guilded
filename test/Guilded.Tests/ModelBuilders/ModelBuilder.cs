namespace Guilded.Tests.ModelBuilders
{
    public abstract class ModelBuilder<T>
    {
        protected T Instance { get; set; }

        protected virtual void BeforeBuild()
        {
        }

        public T Build()
        {
            BeforeBuild();
            return Instance;
        }
    }
}
