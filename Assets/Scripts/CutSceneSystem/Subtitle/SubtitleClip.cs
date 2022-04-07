using UnityEngine;
using UnityEngine.Playables;

namespace CutSceneSystem.Subtitle
{
    public class SubtitleClip : PlayableAsset
    {
        [TextArea(15, 15)]
        public string subtitleText;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SubtitleBehavior>.Create(graph);
            var subtitleBehavior = playable.GetBehaviour();
            subtitleBehavior.SubtitleText = subtitleText;

            return playable;
        }
    }
}
