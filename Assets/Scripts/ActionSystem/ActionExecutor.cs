﻿using System.Collections.Generic;
using EntitySystem;
using JetBrains.Annotations;
using UnityEngine;
using Unstable;
using Unstable.Entities;
using Uxt;
using Uxt.Debugging;

namespace ActionSystem
{
    public class ActionExecutor : StandardComponent, IActionExecutorComponent
    {
        private readonly List<IAction> _pendingActions = new();
        private readonly List<IAction> _activeActions = new();


        private readonly FrameData<Translation> _translationFrame = new();
        public IReadOnlyFrameData<Translation> TranslationFrame => _translationFrame;

        private readonly FrameData<Rotation> _rotationFrame = new();
        public IReadOnlyFrameData<Rotation> RotationFrame => _rotationFrame;

        public void GetAllActions(List<IAction> outActions)
        {
            outActions.AddRange(_activeActions);
        }

        public bool HasPendingOrActiveActions => _pendingActions.Count > 0 || _activeActions.Count > 0;

        public void ExecuteAllActions(float deltaTime)
        {
            BeginPendingActions();
            CallFinishForCompletedActions();
            RemoveCompletedActions();
            UpdateActions();
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

        private void UpdateActions()
        {
            foreach (var action in _activeActions)
            {
                action.Execute(Time.deltaTime);
                _translationFrame.Add(action.TranslationFrame, (lhs, rhs) => lhs + rhs);
                _rotationFrame.Add(action.RotationFrame, (_, rhs) => rhs);

                if (action is MonoBehaviour m)
                {
                    DebugMessageManager.AddOnScreen($"Action ({m.gameObject.name}) - translation: {action.TranslationFrame}",
                        m.GetInstanceID(), Color.blue, 0.01f);
                }
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