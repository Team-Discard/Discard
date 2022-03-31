using UnityEngine;

namespace GameRuleSystem
{
    public class GameRuleManager
    {
        [RuntimeInitializeOnLoadMethod]
        private static void StaticInit()
        {
            
        }

        public void EnactRule(GameRule rule, object src)
        {
            // todo: to:billy
        }

        public void RevokeRule(GameRule rule, object src)
        {
            // todo: to:billy
        }

        public void EnactRule_PlayerCannotMove(Object src) => EnactRule(GameRule.PlayerCannotMove, src);
        public void RevokeRule_PlayerCannotMove(Object src) => RevokeRule(GameRule.PlayerCannotMove, src);
        public void EnactRule_NoHUD(Object src) => EnactRule(GameRule.NoHUD, src);
        public void RevokeRule_NoHUD(Object src) => RevokeRule(GameRule.NoHUD, src);
    }

    public enum GameRule
    {
        PlayerCannotMove,
        NoHUD
    }
}