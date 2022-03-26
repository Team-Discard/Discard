using UnityEngine;
using Unstable;
using Unstable.Entities;
using Uxt;
using Uxt.InterModuleCommunication;

namespace CardSystem
{
    public abstract class Card : ScriptableObject
    {
        public abstract CardUseResult Use(DependencyBag bag);
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Sprite Illustration { get; }
    }
}