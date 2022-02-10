using System.Collections.Generic;
using UnityEngine;
using Unstable.PlayerActions.Charge;

namespace Unstable.Entities
{
    public class PlayerPawnActionExecutor : MonoBehaviour
    {
        [SerializeField] private ChargeAction _chargeActionPrefab;
        [SerializeField] private PlayerPawn _playerPawn;
        [SerializeField] private FallAndPushConfig _fallAndPushConfig;
        
        private List<IAction> _pendingActions;
        private List<IAction> _actions;

        private List<ActionEffects> _actionEffects;

        private void Awake()
        {
            _pendingActions = new List<IAction>();
            _actions = new List<IAction>();

            _actionEffects = new List<ActionEffects>();
        }

        private void Start()
        {
            var action = Instantiate(_chargeActionPrefab, _playerPawn.transform);
            action.Init(_playerPawn);

            _pendingActions.Add(action);
        }

        private void Update()
        {
            _actionEffects.Clear();
            BeginPendingActions();
            CallFinishForCompletedActions();
            RemoveCompletedActions();
            UpdateActions();
            ApplyActionEffects();
        }

        private void CallFinishForCompletedActions()
        {
            foreach (var action in _actions)
            {
                if (action.Completed)
                {
                    action.Finish();
                }
            }
        }

        private void RemoveCompletedActions()
        {
            _actions.RemoveAll(a => a.Completed);
        }

        private void UpdateActions()
        {
            foreach (var action in _actions)
            {
                var effect = action.Execute(Time.deltaTime);
                _actionEffects.Add(effect);
            }
        }

        private void BeginPendingActions()
        {
            foreach (var pendingAction in _pendingActions)
            {
                pendingAction.Begin();
            }

            _actions.AddRange(_pendingActions);
            _pendingActions.Clear();
        }

        private void ApplyActionEffects()
        {
            var translationFrame = new TranslationFrame();
            {
                foreach (var effect in _actionEffects)
                {
                    translationFrame.TargetHorizontalVelocity += effect.HorizontalVelocity;
                }

                var fallAndPush = FallAndPush.Calculate(
                    _fallAndPushConfig.GetOrigin(),
                    _fallAndPushConfig.GetLength(),
                    ~0,
                    -10.0f * Time.deltaTime);

                if (fallAndPush.Type is FallAndPushType.Fall or FallAndPushType.Push)
                {
                    translationFrame.Displacement += fallAndPush.AbsoluteAmount * Vector3.up;
                }
            }
            _playerPawn.SetTranslationFrame(translationFrame);
        }
    }
}