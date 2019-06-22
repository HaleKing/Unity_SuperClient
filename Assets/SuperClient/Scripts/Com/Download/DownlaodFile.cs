using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
//============================================================
//@author	清歌与浊酒
//@create	7/13/2018
//
//@description:下载
//============================================================
namespace Com.Download
{     /// <summary>
      /// 下载
     /// </summary>
	public class DownlaodFile : MonoBehaviour {

        private static DownlaodFile instance;
        public static DownlaodFile _Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("Download").AddComponent<DownlaodFile>();
                }
                return instance;
            }
        }
        private event Action<long> _eventResponseCode = null;

        public Dictionary<string, UnityWebRequest> listRequest = new Dictionary<string, UnityWebRequest>();    //下载请求的列表
        public void ResponseCodeBack(Action<long> back)//网络信息事件
        {
            if (back != null)
                _eventResponseCode += back;
        }
        public DownloadHandler StartDownload(string url, string savePath)
        {
            if (listRequest.ContainsKey(url))
            {
                Debug.Log("下载列表已经存在路径=>" + url);
               
                return null;    //如果已经存在，则返回null
            }
            DownloadHandler loadHandler = new DownloadHandler(savePath);
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.chunkedTransfer = true;
            request.disposeDownloadHandlerOnDispose = true;
            request.SetRequestHeader("Range", "bytes=" + loadHandler.DownedLength + "-"); 
            request.downloadHandler = loadHandler;
        
            request.Send(); //协程操作，可以在协程中调用等待
            listRequest.Add(url, request);   //保存下载的请求
            return loadHandler;
        }

        //停止下载操作
        public void StopDownload(string url)
        {
            UnityWebRequest request = null;
            if (!listRequest.TryGetValue(url, out request))
            {
                Debug.Log("不存在下载的请求=>" + url);
                return;
            }
            listRequest.Remove(url);
            (request.downloadHandler as DownloadHandler).OnDispose();  
            request.Abort();  
            request.Dispose();  

        }

        private void Update()
        {
            //释放下载完成的操作
            List<string> removeList = new List<string>();
            foreach (string url in listRequest.Keys)
            {
                UnityWebRequest request = listRequest[url];
                if (request.isDone)
                {
                    _eventResponseCode.Invoke(request.responseCode);
                    Debug.Log(request.responseCode);
                    request.Dispose();
                    removeList.Add(url);
                }
            }

            for (int i = 0; i < removeList.Count; i++)
            {
                listRequest.Remove(removeList[i]);
            }
            removeList.Clear();
        }

        void OnApplicationQuit()
        {
            //释放下载完成的操作
            foreach (string url in listRequest.Keys)
            {
                (listRequest[url].downloadHandler as DownloadHandler).OnDispose();  //释放资源
                listRequest[url].Dispose();
            }
            listRequest.Clear();
        }
    }
} 