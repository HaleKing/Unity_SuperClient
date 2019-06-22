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
//@description:
//============================================================
namespace Com.Download
{
	public class DownloadHandler : DownloadHandlerScript
    {


        private FileStream fs;


        private int _contentLength = 0;
        private int ContentLength
        {
            get { return _contentLength; }
        }
        
        private int _downedLength = 0;
        public int DownedLength
        {
            get { return _downedLength; }
        }


        private string _fileName;
        public string FileName
        {
            get { return _fileName+".zip"; }
        }


        public string FileNameTemp
        {
            get { return _fileName + ".temp"; }
        }


        private string savePath = null;

        public string DirectoryPath
        {
            get { return savePath.Substring(0, savePath.LastIndexOf('/')); }
        }

        #region 消息通知事件

        private event Action<int> _eventTotalLength = null; 

        private event Action<int> _eventReceivedLength = null; 

        private event Action<float> _eventProgress = null;   

        private event Action<string> _eventComplete = null; 

        #endregion

        #region 注册消息事件
        public void ReceiveddataBack(Action<int> back)
        {
            if (back != null)
                _eventReceivedLength += back;

        }
        public void RegisteReceiveTotalLengthBack(Action<int> back)
        {
            if (back != null)
                _eventTotalLength += back;
        }

        public void RegisteProgressBack(Action<float> back)
        {
            if (back != null)
                _eventProgress += back;
        }

        public void RegisteCompleteBack(Action<string> back)
        {
            if (back != null)
                _eventComplete += back;
        }

        #endregion
        public DownloadHandler(string filePath) : base(new byte[1024 * 200])
        {
            savePath = filePath.Replace('\\', '/');
            _fileName = savePath.Substring(savePath.LastIndexOf('/') + 1); 

            this.fs = new FileStream(savePath + ".temp", FileMode.Append, FileAccess.Write);   
            _downedLength = (int)fs.Length;  
           //fs.Position = _downedLength; //设置继续写入数据的起始位置
        }

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
           if (data == null || data.Length == 0)
            {
                Debug.Log("没有获取到数据缓存！");
                return false;
            }
            fs.Write(data, 0, dataLength);
            _downedLength += dataLength;
        
            if (_eventProgress != null)
                _eventProgress.Invoke((float)_downedLength / _contentLength);   //通知进度消息
            if (_eventReceivedLength != null)
                _eventReceivedLength.Invoke(_downedLength);   //通知进度消息
            return true;
        }

        protected override void CompleteContent()
        {

            string CompleteFilePath = DirectoryPath + "/" + FileName; 
            string TempFilePath = fs.Name;  
            OnDispose();

            if (File.Exists(TempFilePath))
            {
                if (File.Exists(CompleteFilePath))
                {
                    File.Delete(CompleteFilePath);
                }
              File.Move(TempFilePath, CompleteFilePath);
            }
            else
            {
                Debug.Log("生成文件失败=>下载的文件不存在！");
            }
            if (_eventComplete != null)
                _eventComplete.Invoke(CompleteFilePath);

        }

        public void OnDispose()
        {
            fs.Close();
            fs.Dispose();
        }


        protected override void ReceiveContentLength(int contentLength)
        {
            _contentLength = contentLength + _downedLength;
            if (_eventTotalLength != null)
                _eventTotalLength.Invoke(_contentLength);
           
        }
    }
}