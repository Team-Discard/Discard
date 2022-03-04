namespace Unstable
{
    public interface IDamageBox
    {
        int DamageId { get; }
        bool CheckOverlap(HurtBox hurtBox);
        float GetDamageAmount(HurtBox hurtBox);
    }
}