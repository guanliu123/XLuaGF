using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using XLua;

namespace GameMain
{
    //����Xlua��������������Ȼû�����ɵ���ģʽ���������Ƶ���ģʽ��,������GameEntry.Custom��)
    public class XLuaComponent : GameFrameworkComponent
    {
        public XLua.LuaEnv luaenv;

        private LuaFunction LuaUpdate;

        public void InitLuaEvn()
        {
            luaenv = new XLua.LuaEnv();
            if (luaenv == null)
            {
                Log.Error("init lua env error!!");
                return;
            }   
        }

        private void InitLog()
        {
            LuaTable luaLog = luaenv.Global.Get<LuaTable>("Log");

#if ENABLE_LOG || ENABLE_DEBUG_LOG || ENABLE_DEBUG_AND_ABOVE_LOG
            luaLog.Set<string, bool>("LEVEL_DEBUG", true);
#endif
#if ENABLE_LOG||ENABLE_INFO_LOG||ENABLE_DEBUG_AND_ABOVE_LOG || ENABLE_INFO_AND_ABOVE_LOG
            luaLog.Set<string, bool>("LEVEL_INFO", true);
#endif
#if ENABLE_LOG||ENABLE_INFO_LOG||ENABLE_DEBUG_AND_ABOVE_LOG||ENABLE_INFO_AND_ABOVE_LOG || ENABLE_WARNING_AND_ABOVE_LOG
            luaLog.Set<string, bool>("LEVEL_WARNING", true);
#endif
#if ENABLE_LOG||ENABLE_INFO_LOG||ENABLE_DEBUG_AND_ABOVE_LOG||ENABLE_INFO_AND_ABOVE_LOG||ENABLE_WARNING_AND_ABOVE_LOG || ENABLE_ERROR_AND_ABOVE_LOG
            luaLog.Set<string, bool>("LEVEL_ERROR", true);
#endif
#if ENABLE_LOG||ENABLE_INFO_LOG||ENABLE_DEBUG_AND_ABOVE_LOG||ENABLE_INFO_AND_ABOVE_LOG||ENABLE_WARNING_AND_ABOVE_LOG||ENABLE_ERROR_AND_ABOVE_LOG || ENABLE_FATAL_AND_ABOVE_LOG
            luaLog.Set<string, bool>("LEVEL_FATAL", true);
#endif
        }

        //�������ʹ���������ֶ������Ƿ����lua�߼�
        public void StartGame()
        {
            if (luaenv == null) return;
            InitLog();

            LuaTable luaGameMain = luaenv.Global.Get<LuaTable>("GameMain");
            LuaFunction startup = luaGameMain.Get<LuaFunction>("Startup");
            startup.Call();

            LuaUpdate = luaGameMain.Get<LuaFunction>("Update");
        }

        //��ȡlua�еĳ�������
        public int GetLuaConstant(string partname,string name)
        {
            LuaTable Constant = luaenv.Global.Get<LuaTable>("Constant");
            LuaTable part = Constant.Get<LuaTable>(partname);

            return part.Get<int>(name);
        }

        private void OnDestroy()
        {
            if (luaenv != null)
            {
                luaenv.Dispose();
                luaenv = null;
            }
            if (LuaUpdate != null) LuaUpdate = null;
        }
        private void Update()
        {
            if (luaenv != null)
            {
                //�������մ���
                luaenv.Tick();
                if (Time.frameCount % 100 == 0)
                {
                    //ÿ100ִ֡��һ����������������
                    luaenv.FullGc();
                }

                if (LuaUpdate!=null)
                {
                    LuaUpdate.Call(Time.deltaTime, Time.unscaledDeltaTime);
                }                
            }

        }
    }

}