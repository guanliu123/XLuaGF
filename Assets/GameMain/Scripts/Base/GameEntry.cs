using UnityEngine;

namespace GameMain
{
    public partial class GameEntry : MonoBehaviour
    {
        /// <summary>
        /// ��Ϸ���
        /// </summary>
        private void Start()
        {
            InitBuiltinComponents();
            InitCustomComponents();
        }
    }

}
