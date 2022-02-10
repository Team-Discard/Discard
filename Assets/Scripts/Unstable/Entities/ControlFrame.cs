using UnityEngine;
using Unstable.Actions;

namespace Unstable.Entities
{
    public struct ControlFrame
    {
        public Vector2 directionInput;
        public Vector2 controlDirection;
        public ButtonStateFrame jumpKey;
        public ButtonStateFrame eastKey;
        public ButtonStateFrame westKey;
        public ButtonStateFrame southKey;
        public ButtonStateFrame northKey;
    }
}