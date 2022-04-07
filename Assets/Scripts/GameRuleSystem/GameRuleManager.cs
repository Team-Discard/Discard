using System.Collections.Generic;
using UnityEngine;
using Uxt.Utils;

namespace GameRuleSystem
{
    public static class GameRuleManager
    {
        private static Dictionary<GameRule, HashSet<object>> _ruleEnforcers;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void StaticInit()
        {
            _ruleEnforcers = new Dictionary<GameRule, HashSet<object>>();
        }

        public static void EnforceRule(GameRule rule, object src)
        {
            var enforcerSet = _ruleEnforcers.GetOrAdd(rule, () => new HashSet<object>());
            if (!enforcerSet.Add(src))
            {
                Debug.LogError($"The object '{src}' is already enforcing the rule '{rule}'");
            }
        }

        public static void RevokeRule(GameRule rule, object src)
        {
            if (_ruleEnforcers.TryGetValue(rule, out var enforcerSet) && enforcerSet.Remove(src)) return;
            Debug.LogError($"The object '{src}' is not enforcing the rule '{rule}'");
        }

        public static bool IsRuleEnforced(GameRule rule)
        {
            return _ruleEnforcers.TryGetValue(rule, out var enforcers) && enforcers.Count > 0;
        }

        public static void EnforceRule_PlayerCannotMove(Object src) => EnforceRule(GameRule.PlayerCannotMove, src);
        public static void RevokeRule_PlayerCannotMove(Object src) => RevokeRule(GameRule.PlayerCannotMove, src);
        public static void EnforceRule_NoHUD(Object src) => EnforceRule(GameRule.NoHUD, src);
        public static void RevokeRule_NoHUD(Object src) => RevokeRule(GameRule.NoHUD, src);
    }

    public enum GameRule
    {
        PlayerCannotMove,
        NoHUD
    }
}