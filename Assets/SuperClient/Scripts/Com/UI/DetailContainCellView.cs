using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Com.Datafile;
using Com.Config;
using System.IO;
using Com.Download;
using System.Diagnostics;
using UnityEditor;
using System.Reflection;
using UnityEngine.Profiling;
using Com.Network;
using System.Net;
using System;
//============================================================
//@author	清歌与浊酒
//@create	7/12/2018
//
//@description:
//============================================================
namespace Com.UI
{
	public class DetailContainCellView : MonoBehaviour {
        [Header("信息")]
        public long Info;
        [Header("是否检测文件")]
        public bool IsFindfile = true;
        [Header("网络资源地址")]
        public string resourcestr;
        [Header("本地下载地址")]
        public string downloadlink;
        public string loadlink;
        [Header("启动程序名字")]
        public  string applicationName;
        [Header("提示")]
        public GameObject  tipObj;

        [Header("标题")]
        public Text titleNameText;

        [Header("缩略图")]
        public Image courseThumbnailImg;

        [Header("下载组合-下载按钮-进度条-倒计时")]
        public GameObject lockImage;
        public GameObject downBtnObj;
        public Slider progressContainer;
        public Text timeText;
        public Text dataText;
        //本地已经有的版本，可操作的Ui容器
        [Header("按扭组合-打开，删除")]
        // public GameObject btnGroupContainer;
        public GameObject playBtnObj;
        // public Button deleteButton;
        // [Header("下载按扭")]
        private int aggregatedata;//总数据
        private int acceptingofdata;//接受数据
        
        private void Awake()
        {    
            downloadlink = ConfigInfo.downCatchFolderUrl();
            loadlink = ConfigInfo.UncompressOutFolderUrl();
            detection();
        }

        void OnEnable()
        {
            InvokeRepeating("detection", 2f, 0.1f);

        }
        void detection()
        {
            if (IsFindfile)
            {
                FileManager.Instance.FindFile(applicationName, loadlink, (bool isbool) =>
            {
                if (isbool)
                {
                    lockImage.SetActive(false);
                    playBtnObj.SetActive(true);
                }
                else
                {
                    if (!IsCanConnect(resourcestr))
                    {
                        timeText.text = "无效地址";
                    }
                    else { into(); }
                    lockImage.SetActive(true);
                    playBtnObj.SetActive(false);
                }
            });
            }

            
         }
        /// <summary>
        /// 下载按钮
        /// </summary>
        /// <param name="args"></param>
        void downbtn(bool args)
        {
            downBtnObj.SetActive(args);
        }
        /// <summary>
        /// 下载解压缩面板
        /// </summary>
        /// <param name="args"></param>
        void lockImagedown(bool args)
        {

            lockImage.SetActive(args);
        }
        /// <summary>
        /// 开始按钮
        /// </summary>
        /// <param name="args"></param>
        void playbtn(bool args)
        {

            playBtnObj.SetActive(args);
        }
        public void PlayButtonCall()
        {
            Info = 0;
            FileManager.Instance.FindFile(applicationName, loadlink, (bool isbool) =>
            {   if (!isbool)
                {
                    tipObj.SetActive(true);
                    tipObj.GetComponent<AlertsTip>().TipSettings("路径错误");
                }
                FileManager.Instance.FindFile(applicationName, loadlink + applicationName + "/", ".exe", (bool isbools) =>
                {
               

                    if (!isbools)
                    {
                        tipObj.SetActive(true);
                        tipObj.GetComponent<AlertsTip>().TipSettings("打开应用程序失败，无法找到应用程序");
                    }
                    else
                    {
                        if (isbools)
                        { Process.Start(loadlink + applicationName + "/" + applicationName + ".exe"); }

                    }
                });
            });

        }
        public void DownloadButtonCall()
        {if (IsCanConnect(resourcestr))
            {
                IsFindfile = false;
                timeText.text = "链接资源...";
                downbtn(false);
                if (File.Exists(ConfigInfo.downCatchFolderUrl() + applicationName + ".zip"))
                {

                    File.Delete(ConfigInfo.downCatchFolderUrl() + applicationName + ".zip");

                }
                StartDownload(resourcestr, applicationName);
            }
            else
            {
                timeText.text = "无效地址";
                print("地址错误404");
            }
        }
        private static bool IsCanConnect(string url)
        {
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            bool CanCn = true;   //设成可以连接； 
            try
            {
                req = (HttpWebRequest)System.Net.WebRequest.Create(url);
                res = (HttpWebResponse)req.GetResponse();
            }
            catch (Exception)
            {
                CanCn = false;   //无法连接 
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return CanCn;
        }
        public void StartDownload(string url,string FileName)
        {
            DownloadHandler handler = DownlaodFile._Instance.StartDownload(url, downloadlink + FileName);

            if (handler == null)
                return;
           //注册回调
            handler.RegisteProgressBack(Progress1);
            handler.ReceiveddataBack(Takein);
            handler.RegisteReceiveTotalLengthBack(Total);
            handler.RegisteCompleteBack(Complete);
            DownlaodFile._Instance.ResponseCodeBack(ResponseCode);
        }
        private void Total(int length)//要下载的文件总大小
        {
            aggregatedata = length / 1024;
         
        }
        private void Takein(int length)//接收文件总大小
        {
            acceptingofdata = length / 1024;
            dataText.text = length / 1024 + "KB" + " / " + aggregatedata + "KB";
         
        }
        private void Progress1(float progress)//百分比
        {
            progressContainer.value = progress;
            timeText.text =(progress * 100).ToString("0.00") + "%";

        }
        private void Complete(string path)//下载完成
        {

            timeText.text = "正在解压缩";
            ZIP.Instance.CompressingBack(ZIPComplete);
            ZIP.Instance.CompressiPrompt(ZIPTakein);
            StartCoroutine(ZIP.Instance.UnzipWithPath(downloadlink + applicationName + ".zip", loadlink, (bool T) => { }));
        }
        private void ResponseCode(long progress)//下载提示信息
        {
             print(Info);
             Info = progress;
            if (Info != 403 && Info != 0)
            {

            }
            else { timeText.text = "下载失败：" + Info; }

        }
        private void ZIPTakein(string data)//压缩数据
        {

            dataText.text = data;
        }
        private void ZIPComplete(bool isto)//压缩完成
        {
            IsFindfile = true;
            lockImage.SetActive(false);
            playBtnObj.SetActive(true);
            into();
        }
        private void into()//刷新
        {
            timeText.text = "";
            dataText.text = "";
            progressContainer.value = 0;
        }
    }
}