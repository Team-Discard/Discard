using TMPro;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionHintDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _hintText;
        [SerializeField] private RectTransform _rect;
        
        public RectTransform Rect => _rect;
        
        public void SetText(string text)
        {
            _hintText.text = text;
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}