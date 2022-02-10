namespace Unstable.Entities
{
    public interface IPawn
    {
        void SetTranslationFrame(TranslationFrame translationFrame);
        RotationFrame GetRotationFrame();
        void SetRotationFrame(RotationFrame rotationFrame);
    }
}