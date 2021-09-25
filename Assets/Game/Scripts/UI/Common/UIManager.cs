using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Common
{
    public class UIManager: MonoBehaviour
    {
        protected List<UIPanel> Panels;
        
        protected void ResetPanels()
        {
            foreach (var panel in Panels)
            {
                panel.Hide();
            }
        }
    }
}