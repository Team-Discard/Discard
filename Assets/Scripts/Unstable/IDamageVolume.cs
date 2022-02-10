namespace Unstable
{
    public interface IDamageVolume
    {
        int DamageId { get; }
        bool CheckOverlap(DamageablePart damageablePart);
        float GetDamageAmount(DamageablePart damageablePart);
    }
}