using System;

namespace Rigging.Demo.Characters.Player
{
    [Serializable]
    public struct PlayerStatus
    {
        /// <summary>
        /// 現在の体力
        /// </summary>
        public float CurrentHealth;
        
        /// <summary>
        /// 現在の最大体力 (CurrentHealth の上限)
        /// </summary>
        public float CurrentMaxHealth;
        
        /// <summary>
        /// 最大体力 (CurrentMaxHealth の上限)
        /// </summary>
        public float MaxHealth;
        
        /// <summary>
        /// 恐怖値 (Range: 0～1)
        /// </summary>
        public float Fear;
    }
}