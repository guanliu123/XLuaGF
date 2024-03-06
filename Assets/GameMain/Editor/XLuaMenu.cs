using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class XLuaMenu
{
    [MenuItem("XLua/����lua�ļ�Ϊtxt")]
    public static void CopyLuaToTxt()
    {
        string todir = "Assets/GameMain/LuaTxt";
        string fromdir = "Assets/GameMain/LuaScripts";

        //ȫ�����ƣ����������Ŀ��Ŀ¼
        //���ж�Ŀ���ļ����Ƿ����
        if (Directory.Exists(todir))
        {
            DeleteFolder(todir);
        }

        //���������ļ�
        FileUtil.CopyFileOrDirectoryFollowSymlinks(fromdir, todir);

        //ɾ������lua���ļ�
        var notLuaFiles = Directory.GetFiles(todir, "*.*", SearchOption.AllDirectories)
            .Where(f => ".lua" != Path.GetExtension(f)).ToArray();
        if (notLuaFiles != null && notLuaFiles.Length > 0)
        {
            for (int i = 0; i < notLuaFiles.Length; i++)
            {
                File.SetAttributes(notLuaFiles[i], FileAttributes.Normal);
                File.Delete(notLuaFiles[i]);
            }
        }

        //��lua�ļ��ĺ�׺Ϊtxt
        var luafiles = Directory.GetFiles(todir, "*.*", SearchOption.AllDirectories)
            .Where(f => ".lua" == Path.GetExtension(f)).ToArray();
        if (luafiles != null && luafiles.Length > 0)
        {
            for (int i = 0; i < luafiles.Length; i++)
            {
                File.SetAttributes(luafiles[i], FileAttributes.Normal);
                File.Move(luafiles[i], luafiles[i] + ".txt");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("���Ƴɹ�");
    }

    private static void DeleteFolder(string dir)
    {
        //1.�����·��dir�µ��������ļ�
        string[] files = Directory.GetFiles(dir);
        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        //2.�ݹ���Ҵ����dir�µ�������Ŀ¼
        string[] dirs = Directory.GetDirectories(dir);
        foreach (string tdir in dirs)
        {
            DeleteFolder(tdir);//�ݹ���ɾ���ļ��к��ļ����ڵ������ļ�
        }

        //3.ɾ���Լ�
        Directory.Delete(dir, false);
    }
}
