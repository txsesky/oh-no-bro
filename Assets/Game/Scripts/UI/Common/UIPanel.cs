using UnityEngine;

namespace Game.UI.Common
{
    public class UIPanel : MonoBehaviour
    {
        [SerializeField]protected Canvas _canvas;

        public void Show()
        {
            _canvas.enabled = true;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _canvas.enabled = false;
        }

        protected GameObject GetChildWithName(string name)
        {
            var childTrans = gameObject.transform.Find(name);

            return childTrans != null ? childTrans.gameObject : null;
        }
    }
}