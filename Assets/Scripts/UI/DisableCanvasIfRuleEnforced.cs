using GameRuleSystem;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Canvas))]
    // todo: to:billy this is hacky; we also need fade in fade out (polish)
    public class DisableCanvasIfRuleEnforced : MonoBehaviour
    {
        private Canvas _canvas;

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void Update()
        {
            var noHud = GameRuleManager.IsRuleEnforced(GameRule.NoHUD);
            _canvas.enabled = !noHud;
        }
    }
}