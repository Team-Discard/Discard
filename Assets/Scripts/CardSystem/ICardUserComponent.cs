using EntitySystem;
using Uxt.InterModuleCommunication;

namespace CardSystem
{
    public interface ICardUserComponent : IComponent<ICardUserComponent>
    {
        public void UseCard(int index, DependencyBag dependencies);
    }
}