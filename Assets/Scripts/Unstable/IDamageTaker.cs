namespace Unstable
{
    public interface IDamageTaker
    {
        void InteractWithDamageVolume(IDamageVolume damageVolume);
        void ReckonAllDamage(float deltaTime);
    }
}