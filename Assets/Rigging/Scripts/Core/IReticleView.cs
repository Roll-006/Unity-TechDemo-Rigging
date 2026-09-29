using Rigging.Demo.Reticle;

namespace Rigging.Core
{
    public interface IReticleView
    {
        public void SetViewModel(ReticleViewModel viewModel);
        public void OnSetReticleSize(float size);
        public void OnSetAlpha(float alpha);
    }
}