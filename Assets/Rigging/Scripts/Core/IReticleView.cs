namespace Rigging.Core
{
    public interface IReticleView
    {
        public void SetReticleSize(float size);
        
        public void SetTargetAlpha(float alpha);
        
        public void SetMinReticleSize(bool isMinReticleSize);

        public void UpdateAlpha();
    }
}