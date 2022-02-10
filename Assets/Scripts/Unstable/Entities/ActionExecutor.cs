using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Unstable.Entities
{
    public class ActionExecutor
    {
        private readonly List<IAction> _pendingActions = new();
        private readonly List<IAction> _activeActions = new();

        public void GetAllActions(List<IAction> outActions)
        {
            outActions.AddRange(_activeActions);
        }

        public bool HasPendingOrActiveActions => _pendingActions.Count > 0 || _activeActions.Count > 0;

        public void Execute(float deltaTime, List<ActionEffects> outEffects)
        {
            outEffects.Clear();
            BeginPendingActions();
            CallFinishForCompletedActions();
            RemoveCompletedActions();
            UpdateActions(outEffects);
        }

        public void AddAction([NotNull] IAction action)
        {
            Debug.Assert(!_pendingActions.Contains(action), "!_pendingActions.Contains(action)");
            Debug.Assert(!_activeActions.Contains(action), "!_activeActions.Contains(action)");
            
            _pendingActions.Add(action);
        }

        private void CallFinishForCompletedActions()
        {
            foreach (var action in _activeActions)
            {
                if (action.Completed)
                {
                    action.Finish();
                }
            }
        }

        private void RemoveCompletedActions()
        {
            _activeActions.RemoveAll(a => a.Completed);
        }

        private void UpdateActions(List<ActionEffects> outEffects)
        {
            foreach (var action in _activeActions)
            {
                var effect = action.Execute(Time.deltaTime);
                outEffects.Add(effect);
            }
        }

        private void BeginPendingActions()
        {
            foreach (var pendingAction in _pendingActions)
            {
                pendingAction.Begin();
            }

            _activeActions.AddRange(_pendingActions);
            _pendingActions.Clear();
        }
    }
}