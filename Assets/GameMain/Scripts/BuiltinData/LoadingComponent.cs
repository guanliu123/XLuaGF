using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class LoadingComponent : GameFrameworkComponent
    {
        public Transform panel;
        public Slider slider;
        public TextMeshProUGUI text;

        public void SetDesc(string s)
        {
            text.text = s;
        }

        public void SetLoading(float n)
        {
            slider.value = n;
        }

        public void Hide()
        {
            panel.gameObject.SetActive(false);
        }
        public void Show()
        {
            panel.gameObject.SetActive(true);
        }
    }

}
