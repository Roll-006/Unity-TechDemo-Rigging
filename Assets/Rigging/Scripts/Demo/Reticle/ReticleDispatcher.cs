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
        [SerializeField] private ReticleCreationNotifier creationNotifier;
        [SerializeField] private ReticlePresenter presenter;
        [SerializeField] private RectTransform rootRectTransform;
        [SerializeField] private Shooter shooter;

        private GameObject _reticle;

        private void Awake()
        {
            Debugger.ValidateSerializeField(creationNotifier, this);
            Debugger.ValidateSerializeField(presenter, this);
            Debugger.ValidateSerializeField(rootRectTransform, this);
            Debugger.ValidateSerializeField(shooter, this);
        }

        private void Start()
        {
            creationNotifier.CreateReticle
                .Where(x => x != null)
                .Subscribe(OnDispatch)
                .AddTo(this);
        }

        private void OnDispatch(ReticleModel model)
        {
            if (_reticle != null)
            {
                Destroy(_reticle);
            }
            
            _reticle = Instantiate(model.ReticlePrefab, transform);
            var view = _reticle.GetComponent<IReticleView>() ?? 
                       throw new InvalidOperationException($"{model.ReticlePrefab.name} に {nameof(IReticleView)} を継承してください。");
            
            presenter.OnCreateReticle(shooter, model, view);
            view.Initialize(rootRectTransform);
        }
    }
}