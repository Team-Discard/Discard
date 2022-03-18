using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public abstract class ComponentList
    {
        public abstract void Add(IComponent component);
    }

    /// <summary>
    /// Stores a list of <typeparamref name="TComponent"/> <br/>
    /// Provides basic functionalities such as ticking the whole list and cleaning up destroyed elements <br/>
    /// This is recommended over a plain <see cref="List{T}"/>, because it is safe to add or remove elements
    /// during iteration.
    /// </summary>
    /// <typeparam name="TComponent">The type of component this list is storing</typeparam>
    public class ComponentList<TComponent> : ComponentList, IEnumerable<TComponent> where TComponent : IComponent
    {
        private readonly List<TComponent> _list = new();
        private readonly List<TComponent> _pending = new();
        private bool _isTicking;

        /// <summary>
        /// Remove all destroyed components in the list.
        /// </summary>
        public void RemoveDestroyed()
        {
            Debug.Assert(!_isTicking, "Cannot tick the list while ");
            _list.RemoveAll(comp => comp.Destroyed);
        }

        /// <summary>
        /// Iterate over all components in the list and call <paramref name="tickAction"/> on them.
        /// Adding components during iteration will be deferred after the iteration.
        /// </summary>
        /// <param name="deltaTime">The deltaTime passed into <paramref name="tickAction"/></param>
        /// <param name="tickAction">The function to call on each component</param>
        /// <param name="removeDestroyed">If set to true, will remove all destroyed components after iteration</param>
        public void Tick(float deltaTime, Action<TComponent, float> tickAction, bool removeDestroyed = true)
        {
            RemoveDestroyed();
            
            try
            {
                _isTicking = true;
                if (tickAction == null)
                {
                    return;
                }

                foreach (var comp in _list)
                {
                    tickAction.Invoke(comp, deltaTime);
                }
            }
            finally
            {
                _isTicking = false;
            }

            _list.AddRange(_pending);
            _pending.Clear();

            if (removeDestroyed)
            {
                RemoveDestroyed();
            }
        }

        /// <summary>
        /// Add a component to this list.
        /// </summary>
        /// <param name="component">The component to add</param>
        public void Add(TComponent component)
        {
            Debug.Assert(component != null, "component != null");
            if (_isTicking)
            {
                _pending.Add(component);
            }
            else
            {
                _list.Add(component);
            }
        }

        public override void Add(IComponent component)
        {
            Add((TComponent)component);
        }

        public int Count => _list.Count;

        public IEnumerator<TComponent> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }
    }
}