
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class GenerateLuaFileList
{
    [MenuItem("XLua/生成lua文件列表")]
    public static void CopyLuaToTxt()
    {
        string tofile = "/GameMain/Configs/LuaFileList.txt";
        string fromdir = "Assets/GameMain/LuaScripts";

        string path = Application.dataPath + tofile;
        FileStream fileStream;
        StreamWriter sw;
        //获取欲写入文件，如果不存在则创建
        if (!File.Exists(path))
        {
            fileStream = new FileStream(path, FileMode.Create);          
        }
        else
        {
            fileStream = new FileStream(path, FileMode.Open);
        }
        sw = new StreamWriter(fileStream, Encoding.UTF8);

        //获取所有lua文件的路径
        var luafiles = Directory.GetFiles(fromdir, "*.*", SearchOption.AllDirectories)
            .Where(f => ".lua" == Path.GetExtension(f)).ToArray();
        if (luafiles != null && luafiles.Length > 0)
        {
            for (int i = 0; i < luafiles.Length; i++)
            {              
                sw.WriteLine(luafiles[i].Replace("\\", "/"));
            }
        }

        sw.Close();
        fileStream.Close();

        AssetDatabase.Refresh();
    }
}
