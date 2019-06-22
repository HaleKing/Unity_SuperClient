using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
//============================================================
//@author	清歌与浊酒
//@create	6/30/2018
//
//@description:
//============================================================
namespace Com.Master
{
	public class QABEditor  {

        [MenuItem("QFramework/AB/Build")]
        public static void BuildAssetBundle()
        {
            // AB包输出路径
            string outPath = Application.streamingAssetsPath + "/QAB";

            // 检查路径是否存在
            CheckDirAndCreate(outPath);

            BuildPipeline.BuildAssetBundles(outPath, 0, EditorUserBuildSettings.activeBuildTarget);

            // 刚创建的文件夹和目录能马上再Project视窗中出现
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 判断路径是否存在,不存在则创建
        /// </summary>
        public static void CheckDirAndCreate(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
    }
}