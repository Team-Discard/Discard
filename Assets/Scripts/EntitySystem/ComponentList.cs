using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public class ComponentList<TComponent> : IEnumerable<TComponent> where TComponent : IComponent
    {
        private readonly List<TComponent> _list = new();
        private readonly List<TComponent> _pending = new();
        private bool _isTicking;

        public void RemoveDestroyed()
        {
            Debug.Assert(!_isTicking, "Cannot tick the list while ");
            _list.RemoveAll(comp => comp.Destroyed);
        }

        public void Tick(float deltaTime, Action<TComponent, float> tickAction, bool removeDestroyed = true)
        {
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