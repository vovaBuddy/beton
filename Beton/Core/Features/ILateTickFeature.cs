namespace Beton.Core.Features
{
    public interface ILateTickFeature
    {
        void Tick(float deltaTime);
    }
}