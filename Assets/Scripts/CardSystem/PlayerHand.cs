using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace CardSystem
{
    public class PlayerHand
    {
        private const int HandSize = 4;

        private readonly List<Card> _cards = new();

        public PlayerHand()
        {
            for (var i = 0; i < HandSize; ++i)
            {
                _cards.Add(null);
            }
        }

        public Card GetCard(int position)
        {
            Debug.Assert(position is >= 0 and < HandSize, "position is >= 0 and < HandSize");
            return _cards[position];
        }

        public void ClearCard(int position)
        {
            Debug.Assert(position is >= 0 and < HandSize, "position is >= 0 and < HandSize");
            _cards.RemoveAt(position);
        }

        public void SetCard(int position, [NotNull] Card card)
        {
            Debug.Assert(_cards[position] == null);
            _cards[position] = card;
        }
    }
}