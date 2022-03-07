using UnityEngine;
using Unstable;
using Unstable.Entities;

namespace CardSystem
{
    public abstract class Card : ScriptableObject
    {
        public abstract CardUseResult Use(PlayerMasterController playerController);
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Sprite Illustration { get; }
    }
}