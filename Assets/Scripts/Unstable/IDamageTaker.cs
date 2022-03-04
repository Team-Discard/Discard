namespace Unstable
{
    public interface IDamageTaker
    {
        void InteractWithDamageVolume(IDamageBox damageBox);
        void ReckonAllDamage(float deltaTime);
    }
}