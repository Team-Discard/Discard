using System.Collections.Generic;
using UnityEngine;
using Uxt;

namespace CardSystem
{
    public class Deck
    {
        private readonly List<Card> _cards = new();

        public void FillFromTemplate(List<Card> template, bool instantiate = true, bool shuffle = true)
        {
            ClearCards();
            foreach (var card in template)
            {
                Card instance = null;
                if (instantiate)
                {
                    instance = Object.Instantiate(card);
                }
                else
                {
                    instance = card;
                }
                _cards.Add(instance);
            }

            if (shuffle)
            {
                _cards.Shuffle();
            }
        }

        public Card DrawCard()
        {
            if (_cards.Count == 0)
            {
                return null;
            }

            var drawnCard = _cards[^1];
            _cards.RemoveAt(_cards.Count - 1);
            return drawnCard;
        }
        
        private void ClearCards()
        {
            foreach (var card in _cards)
            {
                Object.Destroy(card);
            }

            _cards.Clear();
        }
    }
}