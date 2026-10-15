using System;
using UnityEngine;

namespace Rigging.Core
{
    public static class Debugger
    {
        /// <summary>
        /// SerializeField にオブジェクトが登録されているか検証する。
        /// </summary>
        /// <param name="field">検証するフィールド</param>
        /// <param name="owner">field を所持する Component</param>
        /// <typeparam name="T">field の型</typeparam>
        /// <typeparam name="TU">owner の型</typeparam>
        /// <exception cref="InvalidOperationException">field が null の場合にスローされます。</exception>
        public static void ValidateSerializeField<T, TU>(T field, TU owner)
            where T : UnityEngine.Object
            where TU : Component
        {
            if (field == null)
                throw new InvalidOperationException(
                    $"{typeof(TU).Name} のSerializeField に {typeof(T).Name} を登録してください。\n" + 
                    $"GameObject: {owner.gameObject.name}\n");
        }

        /// <summary>
        /// Component が追加されているか検証する。
        /// </summary>
        /// <param name="gameObject">追加されているか検証する GameObject</param>
        /// <typeparam name="T">追加しているか検証するコンポーネントの型</typeparam>
        /// <returns>追加した Component</returns>
        /// <exception cref="InvalidOperationException">Component が追加されていなかった場合にスローされます。</exception>
        public static T ValidateComponent<T>(GameObject gameObject)
            where T : Component
        {
            return gameObject.GetComponent<T>() ?? 
                   throw new InvalidOperationException($"{gameObject.name} に {typeof(T).Name} を追加してください。");
        }
        
        /// <summary>
        /// Component が追加されているか検証する。
        /// </summary>
        /// <param name="component">追加されているか検証する Component</param>
        /// <typeparam name="T">追加しているか検証するコンポーネントの型</typeparam>
        /// <returns>追加した Component</returns>
        /// <exception cref="InvalidOperationException">Component が追加されていなかった場合にスローされます。</exception>
        public static T  ValidateComponent<T>(Component component)
            where T : Component
        {
            return component.GetComponent<T>() ?? 
                   throw new InvalidOperationException($"{component.gameObject.name} に {typeof(T).Name} を追加してください。");
        }

        /// <summary>
        /// 値の範囲が有効かを検証する。
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <typeparam name="T">検証する値の型</typeparam>
        /// <exception cref="ArgumentOutOfRangeException">min が max 以上であった場合にスローされます。</exception>
        public static void ValidateRange<T>(T min, T max)
            where T : IComparable<T>
        {
            if (min.CompareTo(max) >= 0)
                throw new ArgumentOutOfRangeException(
                    "範囲制限が不正な値です。有効範囲は [ min < max ] です。\n" +
                    $"min: {min}\n" + 
                    $"max: {max}");
        }
    }
}