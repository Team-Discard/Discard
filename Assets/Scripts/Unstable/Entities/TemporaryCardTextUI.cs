using System.Collections.Generic;
using CardSystem;
using UnityEngine;
using Uxt;
using Uxt.Debugging;

namespace Unstable.Entities
{
    /// <summary>
    /// Displays what cards the players have with on screen text
    /// </summary>
    public class TemporaryCardTextUI
    {
        private static readonly List<string> _buttonNames = new ()
        {
            "A", "B", "Y", "X"
        };

        private readonly IReadOnlyList<Card> _cards;
        private readonly List<int> _textHandles;

        public TemporaryCardTextUI(IReadOnlyList<Card> cards)
        {
            _cards = cards;
            _textHandles = new List<int>();
            for (var i = 0; i < cards.Count; ++i)
            {
                var id = DebugMessageManager.AllocateId();
                _textHandles.Add(id);
            }
        }

        public void Tick(float deltaTime)
        {
            var count = Mathf.Min(_cards.Count, _buttonNames.Count);
            for (var i = 0; i < count; ++i)
            {
                var card = _cards[i];
                if (card != null)
                {
                    var cardText = $"({_buttonNames[i]}): {card.Name}";
                    DebugMessageManager.AddOnScreen(cardText, _textHandles[i], Color.red, 0.1f);
                }
            }
        }
    }
}