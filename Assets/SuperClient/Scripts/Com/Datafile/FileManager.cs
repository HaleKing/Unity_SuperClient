using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System;
using UnityEngine.Networking;
using System.Reflection;
using UnityEditor;
using UnityEngine.Profiling;
//============================================================
//@author	清歌与浊酒
//@create	7/9/2018
//
//@description:查找文件
//============================================================
namespace Com.Datafile
{
	public class FileManager : MonoBehaviour
    {
        private event Action<bool> _eventComplete = null;  //完成后的事件回调
        static FileManager instance;
        public static FileManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject mounter = new GameObject("FileManager");
                    instance = mounter.AddComponent<FileManager>();
                }
                return instance;
            }
        }
        /// <summary>
        /// 查找文件夹-回调
        /// </summary>
        /// <param name="filename">文件名字</param>
        /// <param name="path">文件路径</param>
        /// <param name="exist">回调是否查找到</param>
        public void FindFile(string filename,string path,Action<bool> exist)
        {
          
            string newPath = Path.Combine(path, filename);
     
            DirectoryInfo info = new DirectoryInfo(newPath);
            exist(info.Exists);
        }
        /// <summary>
        /// 查找文件-回调
        /// </summary>
        /// <param name="filename">文件名字</param>
        /// <param name="path">文件路径</param>
        /// <param name="exist">回调是否查找到</param>
        public void FindFile(string filename, string path,string suffix, Action<bool> exist)
        {

            exist(File.Exists(path + filename+ suffix));
        }
        /// <summary>
        /// 查找文件夹-返回值
        /// </summary>
        /// <param name="filename">文件名字</param>
        /// <param name="path">文件路径</param>
        /// <param name="isto">弃用</param>
        /// <returns></returns>
        public bool FindFile(string filename, string path,bool isto=true)
        {
            string newPath = Path.Combine(path, filename);

            DirectoryInfo info = new DirectoryInfo(newPath);
      
            return info.Exists;
        }

        private void ii()
        {

       
        }       
    }


}
