namespace Uxt.InterModuleCommunication
{
    public interface IReadOnlyFrameData<T> where T : struct
    {
        public T Value { get; }
    }
}