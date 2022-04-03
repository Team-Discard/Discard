using System;
using System.Collections.Generic;
using CardSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cards
{
    public class HandUIRenderer : MonoBehaviour
    {
        [SerializeField] private List<Image> _cardImages;
        [SerializeField] private CardManager _cardManager;

        // todo: to:billy make this property of the type itself (have separate renderer types for hand and buffer)
        [SerializeField] private CardContainerType _containerToRender;

        private void Start()
        {
            if (_containerToRender == CardContainerType.Hand)
            {
                _cardManager.onStateChanged += RedrawHand;
                RedrawHand();
            }
            else if (_containerToRender == CardContainerType.Buffer)
            {
                _cardManager.onStateChanged += RedrawBuffer;
                RedrawBuffer();
            }
        }

        private void RedrawHand()
        {
            for (var i = 0; i < _cardImages.Count; ++i)
            {
                SetRenderedCard(i, _cardManager.GetCardInHand(i));
            }
        }

        private void RedrawBuffer()
        {
            for (var i = 0; i < _cardImages.Count; ++i)
            {
                SetRenderedCard(i, _cardManager.GetCardInBuffer(i));
            }
        }

        private void SetRenderedCard(int index, Card card)
        {
            if (0 <= index && index < _cardImages.Count)
            {
                if (card == null)
                {
                    _cardImages[index].gameObject.SetActive(false);
                }
                else
                {
                    _cardImages[index].gameObject.SetActive(true);
                    _cardImages[index].sprite = card.Illustration;
                }
            }
        }
    }
}