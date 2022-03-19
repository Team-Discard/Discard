using EntitySystem;
using Uxt;

namespace CardSystem
{
    public interface ICardUser : IComponent<ICardUser>
    {
        public void UseCard(int index, DependencyBag dependencies);
    }
}