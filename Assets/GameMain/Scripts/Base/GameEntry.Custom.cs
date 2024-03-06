using UnityEngine;

namespace GameMain
{
    /// <summary>
    /// ��Ϸ��ڡ�
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static LoadingComponent Loading
        {
            get;
            private set;
        }

        public static XLuaComponent Xlua
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            Loading = UnityGameFramework.Runtime.GameEntry.GetComponent<LoadingComponent>();
            Xlua = UnityGameFramework.Runtime.GameEntry.GetComponent<XLuaComponent>();
        }
    }
}