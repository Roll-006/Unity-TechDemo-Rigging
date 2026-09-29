using System;
using UnityEngine;

namespace Rigging.Demo.Reticle
{
    [Serializable]
    public struct ReticleData
    {
        public ReticleShapeType ShapeType;
        public GameObject ReticlePrefab;
        public float MinSize;
        public float MiddleSize;
        public float MaxSize;
        public float FirstStepEnlargeSpeed;
        public float FirstStepReduceSpeed;
        public float SecondStepEnlargeSpeed;
        public float SecondStepReduceSpeed;
    }
}