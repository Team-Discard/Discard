namespace Uxt.InterModuleCommunication
{
    public interface IReadOnlyFrameData<T> where T : struct
    {
        public bool TryReadValue(out T val);
        bool HasValue => TryReadValue(out _);
    }
}