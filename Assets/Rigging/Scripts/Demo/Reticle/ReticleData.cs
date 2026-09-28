using System;

namespace Rigging.Demo.Reticle
{
    [Serializable]
    public struct ReticleData
    {
        public float MinSize;
        public float MiddleSize;
        public float MaxSize;
        public float FirstStepEnlargeSpeed;
        public float FirstStepReduceSpeed;
        public float SecondStepEnlargeSpeed;
        public float SecondStepReduceSpeed;
    }
}