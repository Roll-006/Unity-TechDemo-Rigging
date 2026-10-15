using UnityEngine;

namespace Rigging.Core
{
    public class Cursor : MonoBehaviour
    {
        [SerializeField] private Texture2D texture;
        [SerializeField] private CursorLockMode cursorLockMode;

        private void Awake()
        {
            var hotspot = texture ? texture.texelSize * 0.5f : Vector2.zero;

            UnityEngine.Cursor.SetCursor(texture, hotspot,  CursorMode.ForceSoftware);
            UnityEngine.Cursor.lockState = cursorLockMode;
        }

        public void OnLock()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }

        public void OnConfined()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        }
    }
}