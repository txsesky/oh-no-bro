using UnityEngine;

namespace Game.UI.Common
{
    public class Panel : MonoBehaviour
    {
        protected Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void OnDestroy()
        {
            _canvas = null;
        }

        public void Show()
        {
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }

        protected GameObject GetChildWithName(string name)
        {
            var childTrans = gameObject.transform.Find(name);

            return childTrans != null ? childTrans.gameObject : null;
        }
    }
}