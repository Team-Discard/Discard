using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Uxt.InterModuleCommunication;
using Uxt.Utils;

namespace CardSystem
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private int _size;
        private List<Card> _hand;
        private List<Card> _buffer;

        public Action onStateChanged;

        private void Awake()
        {
            _hand = ListUtility.MakeEmptyList<Card>(_size);
            _buffer = ListUtility.MakeEmptyList<Card>(_size);
            onStateChanged = null;
        }

        private bool IsHandFull => IsListFull(_hand);
        private bool IsBufferFull => IsListFull(_buffer);

        public Card GetCardInHand(int index)
        {
            Debug.Assert(index >= 0 && index < _size);
            return _hand[index];
        }

        public Card GetCardInBuffer(int index)
        {
            Debug.Assert(index >= 0 && index < _size);
            return _buffer[index];
        }

        public CardLocation? AcquireCard(Card card, int preferredIndex)
        {
            CardLocation? location = null;
            
            if (!IsHandFull)
            {
                location = new CardLocation(CardContainerType.Hand, InsertCard(_hand, card, preferredIndex));
            }
            else if (!IsBufferFull)
            {
                location = new CardLocation(CardContainerType.Buffer, InsertCard(_buffer, card, preferredIndex));
            }

            if (location != null)
            {
                onStateChanged?.Invoke();
            }
            
            return location;
        }

        private static bool IsListFull(List<Card> list) => list.TrueForAll(c => c != null);

        private int InsertCard(List<Card> list, Card card, int preferredIndex)
        {
            Debug.Assert(!IsListFull(list));

            var iterationCount = 0;
            while (list[preferredIndex] != null && iterationCount <= list.Count)
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

            onStateChanged?.Invoke();

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