namespace CombatSystem
{
    /// <summary>
    /// Entities on the same damage layer
    /// would not deal damage to each other
    /// </summary>
    public enum FriendLayer
    {
        // todo: to:billy research whether we can change enum int value safely without breaking things in the editor
        
        Player = 0,
        Enemy = 1,
        Environment = 2
    }
}