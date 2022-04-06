using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Uxt.PropertyDrawers;

namespace GameRuleSystem
{
    public class GameRuleEnforcer : MonoBehaviour
    {
        [SerializeField, EditInPrefabOnly] private List<GameRule> _rules;

        private void OnEnable()
        {
            Debug.Assert(_rules.ToHashSet().Count == _rules.Count, "The rule enforcer contains duplicate rules",
                gameObject);

            foreach (var rule in _rules)
            {
                GameRuleManager.EnforceRule(rule, this);
            }
        }

        private void OnDisable()
        {
            foreach (var rule in _rules)
            {
                GameRuleManager.RevokeRule(rule, this);
            }
        }
    }
}