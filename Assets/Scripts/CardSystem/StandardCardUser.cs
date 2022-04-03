using ActionSystem;
using EntitySystem;
using Uxt.InterModuleCommunication;

namespace CardSystem
{
    public class StandardCardUser : StandardComponent, ICardUserComponent
    {
        private readonly IActionExecutorComponent _actionExecutor;
        private readonly CardManager _cardManager;

        public StandardCardUser(IActionExecutorComponent actionExecutor, CardManager cardManager)
        {
            _actionExecutor = actionExecutor;
            _cardManager = cardManager;
        }

        public void UseCard(int index, DependencyBag dependencies)
        {
            if (_actionExecutor.HasPendingOrActiveActions) return;
            if (_cardManager.GetCardInHand(index) == null) return;

            var useResult = _cardManager.UseCard(index, dependencies);
            var action = useResult.Action;
            if (action != null)
            {
                _actionExecutor.AddAction(useResult.Action);
            }
        }
    }
}