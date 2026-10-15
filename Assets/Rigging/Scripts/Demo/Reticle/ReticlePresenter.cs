using R3;
using Rigging.Core;
using Rigging.Demo.Characters.Player;
using UnityEngine;

namespace Rigging.Demo.Reticle
{
    public class ReticlePresenter : MonoBehaviour
    {
        /// <summary>
        /// レティクルが生成されたら Bind する。
        /// </summary>
        /// <param name="shooter">シューター (Model)</param>
        /// <param name="model">生成されたレティクルの Model</param>
        /// <param name="view">生成されたレティクルの View</param>
        public void OnCreateReticle(Shooter shooter, ReticleModel model, IReticleView view)
        {
            view.SetObstacleReticleSize(model.ObstacleSize);
            
            model.CurrentSize.Subscribe(view.SetReticleSize).AddTo(this);
            model.IsReticleClosed.Subscribe(view.EnableCenterPoint).AddTo(this);
            model.ObstaclePosition.Subscribe(view.SetObstaclePosition).AddTo(this);
            shooter.IsAiming.Subscribe(view.SetAimState).AddTo(this);
            
            shooter.IsAiming.Subscribe(isAiming =>
            {
                view.SetTargetAlpha(isAiming ? 1f : 0f);
            }).AddTo(this);
        }
    }
}