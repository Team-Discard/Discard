using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Uxt.InterModuleCommunication;
using Uxt.Utils;

namespace CardSystem
{
    public class CardManager
    {
        private readonly int _size;
        private readonly List<Card> _hand;
        private readonly List<Card> _buffer;

        public Action onStateChanged;

        public CardManager(int size)
        {
            _size = size;
            _hand = ListUtility.MakeEmptyList<Card>(size);
            _buffer = ListUtility.MakeEmptyList<Card>(size);
            onStateChanged = null;
        }

        private bool IsHandFull => IsListFull(_hand);
        private bool IsBufferFull => IsListFull(_buffer);

        public Card GetCardInHand(int index)
        {
            Debug.Assert(index >= 0 && index < _size);
            return _hand[index];
        }

        public CardLocation? AcquireCard(Card card, int preferredIndex)
        {
            if (!IsHandFull)
            {
                return new CardLocation(CardContainerType.Hand, InsertCard(_hand, card, preferredIndex));
            }
            else if (!IsBufferFull)
            {
                return new CardLocation(CardContainerType.Buffer, InsertCard(_buffer, card, preferredIndex));
            }
            else
            {
                return null;
            }
        }

        private static bool IsListFull(List<Card> list) => list.TrueForAll(c => c != null);
        
        private static int InsertCard(List<Card> list, Card card, int preferredIndex)
        {
            Debug.Assert(!IsListFull(list));
            
            var iterationCount = 0;
            while (list[preferredIndex] != null && iterationCount < list.Count)
            {
                preferredIndex = (preferredIndex + 1) % list.Count;
                ++iterationCount;
            }

            Debug.Assert(list[preferredIndex] == null);
            list[preferredIndex] = card;
            return preferredIndex;
        }

        public CardUseResult UseCard(int index, DependencyBag userDependencies)
        {
            Debug.Assert(index >= 0 && index < _size);
            Debug.Assert(_hand[index] != null);

            var card = _hand[index];
            _hand[index] = null;
            if (_hand.All(c => c == null))
            {
                MoveBufferToHand();
            }

            return card.Use(userDependencies);
        }

        private void MoveBufferToHand()
        {
            for (var i = 0; i < _size; ++i)
            {
                _hand[i] = _buffer[i];
                _buffer[i] = null;
            }
        }
    }

    public enum CardContainerType
    {
        Hand,
        Buffer
    }

    public struct CardLocation
    {
        public CardContainerType container;
        public int index;

        public CardLocation(CardContainerType container, int index)
        {
            this.container = container;
            this.index = index;
        }
    }
}