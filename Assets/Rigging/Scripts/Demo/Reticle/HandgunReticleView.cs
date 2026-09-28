using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Reticle
{
    public class HandgunReticleView : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform left;
        [SerializeField] private RectTransform right;
        [SerializeField] private RectTransform top;
        [SerializeField] private RectTransform bottom;

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(rectTransform, this);

            ChangeSize();
        }

        private void ChangeSize()
        {
            if (left != null) left.anchoredPosition = new Vector2(-left.sizeDelta.x / 2f, 0f);
            if (right != null) right.anchoredPosition = new Vector2(right.sizeDelta.x / 2f, 0f);
            if (top != null) top.anchoredPosition = new Vector2(0f, top.sizeDelta.y / 2f);
            if (bottom != null) bottom.anchoredPosition = new Vector2(0f, -bottom.sizeDelta.y / 2f);
        }
    }
}