using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class EnforceOptionTextColor : MonoBehaviour
    {
        [SerializeField] private Color textColor;

        private void Update()
        {
            var textList = GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var t in textList)
            {
                t.color = textColor;
            }
        }
    }
}
