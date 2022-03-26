using EntitySystem;
using Uxt;

namespace CardSystem
{
    public interface ICardUserComponent : IComponent<ICardUserComponent>
    {
        public void UseCard(int index, DependencyBag dependencies);
    }
}