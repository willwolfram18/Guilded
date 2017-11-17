namespace Guilded.Tests.ModelBuilders
{
    public abstract class ModelBuilder<T>
    {
        protected T Instance { get; set; }

        public T Build() => Instance;
    }
}
