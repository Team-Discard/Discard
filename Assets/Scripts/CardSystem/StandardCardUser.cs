using System.Collections.Generic;
using System.Linq;
using ActionSystem;
using EntitySystem;
using UnityEngine;
using Uxt;

namespace CardSystem
{
    public class StandardCardUser : StandardComponent, ICardUser
    {
        private readonly IActionExecutor _actionExecutor;
        private readonly List<Card> _hand;

        public StandardCardUser(IActionExecutor actionExecutor, IEnumerable<Card> hand)
        {
            _actionExecutor = actionExecutor;
            _hand = hand.ToList();
        }

        public void UseCard(int index, DependencyBag dependencies)
        {
            Debug.Assert(0 <= index && index < _hand.Count);

            if (_actionExecutor.HasPendingOrActiveActions)
            {
                return;
            }

            var useResult = _hand[index].Use(dependencies);
            var action = useResult.Action;
            if (action != null)
            {
                _actionExecutor.AddAction(useResult.Action);
            }
        }
    }
}