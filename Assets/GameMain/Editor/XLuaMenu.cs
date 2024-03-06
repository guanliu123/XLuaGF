using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class XLuaMenu
{
    [MenuItem("XLua/复制lua文件为txt")]
    public static void CopyLuaToTxt()
    {
        string todir = "Assets/GameMain/LuaTxt";
        string fromdir = "Assets/GameMain/LuaScripts";

        //全部复制，所以先清空目标目录
        //先判断目标文件夹是否存在
        if (Directory.Exists(todir))
        {
            DeleteFolder(todir);
        }

        //拷贝所有文件
        FileUtil.CopyFileOrDirectoryFollowSymlinks(fromdir, todir);

        //删除不是lua的文件
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

        //改lua文件的后缀为txt
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
        Debug.Log("复制成功");
    }

    private static void DeleteFolder(string dir)
    {
        //1.传入的路径dir下的所有子文件
        string[] files = Directory.GetFiles(dir);
        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        //2.递归查找传入的dir下的所有子目录
        string[] dirs = Directory.GetDirectories(dir);
        foreach (string tdir in dirs)
        {
            DeleteFolder(tdir);//递归来删除文件夹和文件夹内的所有文件
        }

        //3.删除自己
        Directory.Delete(dir, false);
    }
}
