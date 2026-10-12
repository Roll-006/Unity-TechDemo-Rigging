using System;
using R3;
using UnityEngine;

namespace Rigging.Demo.Reticle
{
    [Serializable]
    public class ReticleModel : IDisposable
    {
        public ReactiveProperty<float> CurrentSize = new ReactiveProperty<float>();
        public ReactiveProperty<bool> IsReticleClosed = new ReactiveProperty<bool>();
        
        public ReticleShapeType ReticleShapeType;
        public GameObject ReticlePrefab;
        public float MinSize;
        public float MiddleSize;
        public float MaxSize;
        public float FirstStepEnlargeSpeed;
        public float FirstStepReduceSpeed;
        public float SecondStepEnlargeSpeed;
        public float SecondStepReduceSpeed;

        public void Dispose()
        {
            CurrentSize.Dispose();
            IsReticleClosed.Dispose();
        }
    }
}