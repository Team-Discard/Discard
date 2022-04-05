using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace CutSceneSystem.Subtitle
{
    public class SubtitleTrackMixer : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var tmp = playerData as TextMeshProUGUI;

            if (tmp == null)
            {
                return;
            }
            
            var currentText = "";
            var currentAlpha = 0f;

            var inputCount = playable.GetInputCount();
            for (var i = 0; i < inputCount; i++)
            {
                var inputWeight = playable.GetInputWeight(i);
                if (!(inputWeight > 0f)) continue;
                
                var inputPlayable =
                    (ScriptPlayable<SubtitleBehavior>) playable.GetInput(i);
                var input = inputPlayable.GetBehaviour();
                currentText = input.SubtitleText;
                currentAlpha = inputWeight;
            }

            tmp.text = currentText;
            tmp.color = new Color(1f, 1f, 1f, currentAlpha);
        }
    }
}

