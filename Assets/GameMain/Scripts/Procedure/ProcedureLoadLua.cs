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
                 �� Windows ϵͳ�У��ı��ļ����н�����ͨ���� `\r\n`������ Unix �� Linux ϵͳ���� `\n`��
                 ����ı��ļ����� Windows ϵͳ�д����ģ���ô��ʹ�� `Split('\n')` ����ʱ��ÿһ�е�ĩβ���ܻ�����һ�� `\r` �ַ���
                 Ϊ�˽��������⣬������ļ�·�����б�֮ǰ���ȶ�ÿһ�н��� `Trim` ���������Ƴ��κο��ܵĿհ��ַ������� `\r`
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

            //�ڱ༭��ģʽ��ʹ�ñ��ؼ���
            if (GameEntry.Base.EditorResourceMode)
            {
                LoadLuaFile();
            }
            else//AB�������ļ���
            {
                LoadLuaFile(0);
            }
        }

        //�༭��ģʽ��
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

        //AB����ʹ��,AB���м�����Դ��unity���к�׺�����ƣ�lua��׺����֧��
        //����ͨ��lua�ű�����ΪTextAsset���ʹ����������ʱִ��
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

