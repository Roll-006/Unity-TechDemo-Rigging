using System;
using R3;
using Rigging.Core;
using Rigging.Demo.Player;
using UnityEngine;

namespace Rigging.Demo.Reticle
{
    /// <summary>
    /// レティクルの生成を通知して、 View を割り当てる。
    /// </summary>
    public class ReticleDispatcher : MonoBehaviour
    {
        [SerializeField] private ReticleLifecycleNotifier lifecycleNotifier;
        [SerializeField] private ReticlePresenter presenter;
        [SerializeField] private Shooter shooter;

        private GameObject _reticle;
        
        private void Start()
        {
            lifecycleNotifier.ReticleLifecycle
                .Where(x => x != null)
                .Subscribe(Dispatch)
                .AddTo(this);
        }

        private void Dispatch(ReticleModel model)
        {
            if (_reticle != null)
            {
                Destroy(_reticle);
            }
            
            _reticle = Instantiate(model.ReticlePrefab, transform);
            var view = _reticle.GetComponent<IReticleView>() ?? 
                       throw new InvalidOperationException($"{model.ReticlePrefab.name} に {nameof(IReticleView)} を継承してください。");
            
            presenter.OnCreateReticle(shooter, model, view);
        }
    }
}