using System;

namespace Annotations
{
    [AttributeUsage( AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class FeatureAttribute : Attribute
    {
        public FeatureTag Tag { get; }

        public FeatureAttribute(FeatureTag tag)
        {
            Tag = tag;
        }
    }

    public enum FeatureTag
    {
        CameraController,
        PlayerController,
        PlayerInputHandler,
    } 
}