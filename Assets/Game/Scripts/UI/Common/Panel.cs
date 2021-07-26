using UnityEngine;

namespace Game.UI.Common
{
    public class Panel : MonoBehaviour
    {
        protected GameObject GetChildWithName(string name)
        {
            var childTrans = gameObject.transform.Find(name);

            return childTrans != null ? childTrans.gameObject : null;
        }
    }
}