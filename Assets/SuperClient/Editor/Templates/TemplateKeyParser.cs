using UnityEngine;
using UnityEditor;
using System.IO;

namespace SuperClient.Templates
{
    public class TemplateKeyParser : UnityEditor.AssetModificationProcessor
    {
        private static string AUTHOR = "清歌与浊酒";
        public static void OnWillCreateAsset(string path)
        {
            Debug.Log("TempleteKeyParser=>" + path);
            path = path.Replace(".meta", "");
            int index = path.LastIndexOf(".");
            //没有后缀名或者是文件夹或者不是.cs文件
            if (index<0||path.Substring(index).Equals(".cs") == false) return;
            //
            index = Application.dataPath.LastIndexOf("Assets");
            string fullPath = Application.dataPath.Substring(0, index) + path;
            Debug.Log("full path => " + fullPath);
            //
            index = path.LastIndexOf("Scripts/");
         
            if (index > 0)
            {
                index += 8;
            }
            else
            {
                index = path.LastIndexOf("Editor/");
              
                if (index > 0)
                {
                    index += 7;
                }
                else
                {
                    return;
                }
            }

            path = path.Substring(index,path.LastIndexOf("/")-index); 
            string ns = path.Replace("/",".");
            Debug.Log("names => " + ns);
            //
            string file = File.ReadAllText(fullPath);
            //
            file = file.Replace("#AUTHOR#", AUTHOR);
            file = file.Replace("#CREATEDATE#", System.DateTime.Now.ToString("d"));
            file = file.Replace("#NAMESPACE#", ns);
            //file = file.Replace("#PROJECTNAME#", PlayerSettings.productName);
            //file = file.Replace("#DEVELOPERS#", PlayerSettings.companyName);
            //file = file.Replace("#FILEEXTENSION#", fileExtension);
            //
            File.WriteAllText(fullPath, file);
            AssetDatabase.Refresh();
        }
    }
}

