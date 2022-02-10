using System;

namespace Unstable.Entities
{
    [AttributeUsage(validOn: AttributeTargets.Field | AttributeTargets.Property)]
    public class FrameDataAttribute : Attribute
    {
        public FrameDataType Type { get; }

        public FrameDataAttribute(FrameDataType type)
        {
            Type = type;
        }
    }
}