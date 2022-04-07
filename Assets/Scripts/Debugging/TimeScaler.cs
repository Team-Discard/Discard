using JetBrains.Annotations;
using UnityEngine;
using Uxt.Debugging;

namespace Debugging
{
    public class TimeScaler
    {
        [DebugCommand, UsedImplicitly]
        public void ChangeTimeScale(float scale)
        {
            scale = Mathf.Clamp(scale, 0.0f, 5.0f);
            Time.timeScale = scale;
            DebugConsole.PrintMessage($"[Time scaler] Changed time scale to {scale}");
        }

        [DebugCommand, UsedImplicitly]
        public void ChangeTimeScale()
        {
            if (Mathf.Approximately(Time.timeScale, 1.0f))
            {
                ChangeTimeScale(5.0f);
            }
            else
            {
                ChangeTimeScale(1.0f);
            }
        }
    }
}