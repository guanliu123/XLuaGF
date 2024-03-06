using GameFramework.DataTable;
using GameFramework.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XLua;

namespace GameMain
{
    public static class UIExtension
    {
        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }

        public static IEnumerator SmoothValue(this Slider slider, float value, float duration)
        {
            float time = 0f;
            float originalValue = slider.value;
            while (time < duration)
            {
                time += Time.deltaTime;
                slider.value = Mathf.Lerp(originalValue, value, time / duration);
                yield return new WaitForEndOfFrame();
            }

            slider.value = value;
        }

        public static void CloseUIForm(this UIComponent uiComponent, UGuiForm uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }

        public static int? OpenUIForm(this UIComponent uiComponent,  LuaTable userData)
        {
            string assetName = userData.Get<string>("assetName");

            //是否允许打开同样的界面
            if (!userData.Get<bool>("AllowMultiInstance"))
            {
                if (uiComponent.IsLoadingUIForm(assetName))
                {
                    return null;
                }

                if (uiComponent.HasUIForm(assetName))
                {
                    return null;
                }
            }

            
            //组名就是将打开的ui指定放入哪个组
            //第二个参数是优先级，也是从lua表中获取
            return uiComponent.OpenUIForm(assetName,
                userData.Get<string>("UIGroupName"),
                GameEntry.Xlua.GetLuaConstant("AssetPriority", "UIFormAsset"), 
                userData.Get<bool>("PauseCoveredUIForm"),
                userData);
        }
    }
}