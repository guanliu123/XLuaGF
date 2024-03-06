using GameFramework;
using GameFramework.Procedure;
using GameFramework.Resource;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityGameFramework.Runtime;
using XLuaTest;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedureLoadLua : ProcedureBase
    {
        private List<string> luaFileList = new List<string>();

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            GameEntry.Xlua.InitLuaEvn();

            LoadAssetCallbacks callbacks = new LoadAssetCallbacks(OnLoadLuaFilesConfigSuccess, OnLoadLuaFilesConfigFail);
            string assetName = "Assets/GameMain/Configs/LuaFileList.txt";
            GameEntry.Resource.LoadAsset(assetName, callbacks);
        }
        private void OnLoadLuaFilesConfigSuccess(string assetName, object asset, float duration, object userData)
        {
            TextAsset textAsset = (TextAsset)asset;
            Log.Info("Load LuaFileList:{0}", assetName);

            string content = textAsset.text;
            ParseLuaFileList(content);
        }
        private void ParseLuaFileList(string content)
        {
            luaFileList.Clear();

            string[] contentLines = content.Split('\n');
            int len = contentLines.Length;
            for(int i = 0; i < len; i++)
            {
                /*
                 在 Windows 系统中，文本文件的行结束符通常是 `\r\n`，而在 Unix 或 Linux 系统中是 `\n`。
                 如果文本文件是在 Windows 系统中创建的，那么在使用 `Split('\n')` 方法时，每一行的末尾可能会留下一个 `\r` 字符。
                 为了解决这个问题，在添加文件路径到列表之前，先对每一行进行 `Trim` 操作，以移除任何可能的空白字符，包括 `\r`
                 */
                string line = contentLines[i].Trim();
                if (!string.IsNullOrEmpty(line))
                {
                    luaFileList.Add(line);
                }
            }

            if(luaFileList.Count==0)
            {
                Log.Error("lua file list is empty");
                return;
            }

            //在编辑器模式下使用本地加载
            if (GameEntry.Base.EditorResourceMode)
            {
                LoadLuaFile();
            }
            else//AB包打包后的加载
            {
                LoadLuaFile(0);
            }
        }

        //编辑器模式下
        private void LoadLuaFile()
        {
            int index = 0;
            while (index < luaFileList.Count)
            {
                string assetName = luaFileList[index];
                GameEntry.Xlua.luaenv.DoString(string.Format("dofile('{0}')", assetName));  
                GameEntry.Loading.SetLoading((index+1)/luaFileList.Count);
                index++;
            }
            GameEntry.Xlua.StartGame();
        }

        //AB包中使用,AB包中加载资源，unity中有后缀名限制，lua后缀名不支持
        //所以通常lua脚本是作为TextAsset类型打包，在运行时执行
        private void LoadLuaFile(int index)
        {
            if (index == luaFileList.Count)
            {             
                GameEntry.Loading.SetDesc("LoadingComplete");
                GameEntry.Xlua.StartGame();
                return;
            }

            string assetName = luaFileList[index].Replace("LuaScripts", "LuaTxt") +".txt";
            LoadAssetCallbacks callbacks = new LoadAssetCallbacks(OnLoadLuaFilesSuccess, OnLoadLuaFilesFail);
            GameEntry.Resource.LoadAsset(assetName, callbacks ,index);
        }
        private void OnLoadLuaFilesSuccess(string assetName, object asset, float duration, object userData)
        {
            Log.Info("load lua {0} success", assetName);
            TextAsset textAsset = (TextAsset)asset;
            GameEntry.Xlua.luaenv.DoString(textAsset.text);

            int index=(int)userData;
            GameEntry.Loading.SetLoading((index + 1) / luaFileList.Count);

            LoadLuaFile(index + 1);
        }


        private void OnLoadLuaFilesFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            throw new NotImplementedException();
        }
        private void OnLoadLuaFilesConfigFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            throw new NotImplementedException();
        }
    }
}

