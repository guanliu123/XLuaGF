using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XLua;

namespace GameMain
{
    public class UGuiForm : UIFormLogic
    {
        //保存从lua传过来的table
        private LuaTable m_userData;
        private LuaFunction LuaUpdate;
        public const int DepthFactor = 100;
        private const float FadeTime = 0.3f;

        private static Font s_MainFont = null;
        private Canvas m_CachedCanvas = null;
        private CanvasGroup m_CanvasGroup = null;
        private List<Canvas> m_CachedCanvasContainer = new List<Canvas>();

        public int OriginalDepth
        {
            get;
            private set;
        }

        public int Depth
        {
            get
            {
                return m_CachedCanvas.sortingOrder;
            }
        }

        public void Close()
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            StopAllCoroutines();

            if (ignoreFade)
            {
                GameEntry.UI.CloseUIForm(this);
            }
            else
            {
                StartCoroutine(CloseCo(FadeTime));
            }
        }

        /*public void PlayUISound(int uiSoundId)
        {
            GameEntry.Sound.PlayUISound(uiSoundId);
        }*/

        public static void SetMainFont(Font mainFont)
        {
            if (mainFont == null)
            {
                Log.Error("Main font is invalid.");
                return;
            }

            s_MainFont = mainFont;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_userData = userData as LuaTable;
            //如果传入的userData里有update方法则要每帧调用
            LuaUpdate = m_userData.Get<LuaFunction>("Update");

            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            m_CachedCanvas.overrideSorting = true;
            OriginalDepth = m_CachedCanvas.sortingOrder;

            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();

            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;

            gameObject.GetOrAddComponent<GraphicRaycaster>();

            LuaFunction LuaOnInit = m_userData.Get<LuaFunction>("OnUInit");
            if (LuaOnInit != null)
            {
                LuaOnInit.Call(m_userData,this,transform,gameObject);
            }
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_CanvasGroup.alpha = 0f;
            StopAllCoroutines();
            StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));

            LuaFunction LuaOnOpen = m_userData.Get<LuaFunction>("OnOpen");
            if (LuaOnOpen != null)
            {
                LuaOnOpen.Call(m_userData);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            LuaFunction LuaOnClose = m_userData.Get<LuaFunction>("OnClose");
            if (LuaOnClose != null)
            {
                LuaOnClose.Call(m_userData);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();

            m_CanvasGroup.alpha = 0f;
            StopAllCoroutines();
            StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));
        }

        protected override void OnCover()
        {
            base.OnCover();
        }


        protected override void OnReveal()
        {
            base.OnReveal();
        }

        protected override void OnRefocus(object userData)
        {
            base.OnRefocus(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (LuaUpdate != null)
            {
                //因为是冒号方法直接调用，要把自己作为第一个参数传回去
                LuaUpdate.Call(m_userData,elapseSeconds, realElapseSeconds);
            }
        }

        private void OnDestroy()
        {
            try
            {
                LuaFunction LuaDestroy=m_userData.Get<LuaFunction>("OnDestroy");
                if (LuaDestroy == null)
                {
                    Log.Error("OnDestroy is not found.");
                }
                else
                {
                    //因为是冒号方法直接调用，要把自己作为第一个参数传回去
                    LuaDestroy.Call(m_userData);
                }

                m_userData.Dispose();
            }
            catch
            {

            }
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            int oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            int deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            GetComponentsInChildren(true, m_CachedCanvasContainer);
            for (int i = 0; i < m_CachedCanvasContainer.Count; i++)
            {
                m_CachedCanvasContainer[i].sortingOrder += deltaDepth;
            }

            m_CachedCanvasContainer.Clear();
        }

        private IEnumerator CloseCo(float duration)
        {
            yield return m_CanvasGroup.FadeToAlpha(0f, duration);
            GameEntry.UI.CloseUIForm(this);
        }
    }
}