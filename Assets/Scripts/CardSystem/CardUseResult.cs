using Unstable;

namespace CardSystem
{
    public struct CardUseResult
    {
        public bool ConsumesCard { get; set; }
        public IAction Action { get; set; }
    }
}
