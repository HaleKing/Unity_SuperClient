using UnityEngine;
using System.Collections;
using Com.Datafile;
using Com.UI;
using System.Diagnostics;
using System;
using UnityEngine.UI;
//============================================================
//@author	清歌与浊酒
//@create	7/12/2018
//
//@description:
//============================================================
namespace Com.Master
{
	public class RAII : MonoBehaviour {
        public GameObject alertsTip;
        public GameObject tooltip;
        public Text tooltiptext;
        [Header("是否开启了外部应用程序默认关闭")]
        public bool IsSoftware=false;
        [Header("开启应用程序名字默认空")]
        public string applicationName = null;
        private void Awake()
        {
           
           

        }
		// Use this for initialization
		void Start () {
	
		}
	
		// Update is called once per frame
		void Update ()
        {
            FileManager.Instance.FindFile("datadirectories", Application.dataPath + "/../", (bool isbool) =>
            {
                if (!isbool)
                {
                    if (!alertsTip.active)
                    {
                       // Debug.LogError(isbool);
                        alertsTip.SetActive(true);
                        alertsTip.GetComponent<AlertsTip>().TipSettings("程序启动失败(缺少必要的资源包)，请点击确定退出");
                    }
                }
            });
            FileManager.Instance.FindFile("common", Application.dataPath + "/../datadirectories/", (bool isbool) =>
            {
                if (!isbool)
                {

                    if (!alertsTip.active)
                    {
                       // Debug.LogError(Application.dataPath + "/../datadirectories/");
                        alertsTip.SetActive(true);
                        alertsTip.GetComponent<AlertsTip>().TipSettings("程序启动失败(缺少必要的资源包)，请点击确定退出");
                    }
                }

            });

            if (IsSoftware&& applicationName!=null)
            {
                if (CheckProcess(applicationName))
                {
                    tooltip.SetActive(true);
                    tooltiptext.text = "应用程序正在运行，如要操作，请关闭以打开的应用程序";

                }
            }
        }
        bool CheckProcess(string processName)
        {
            bool isRunning = false;
            Process[] processes = Process.GetProcesses();
            int i = 0;
            foreach (Process process in processes)
            {
                try
                {
                    i++;
                    if (!process.HasExited)
                    {
                        if (process.ProcessName.Contains(processName))
                        {
                            UnityEngine.Debug.Log(processName + "正在运行");
                            isRunning = true;
                            continue;
                        }
                        else if (!process.ProcessName.Contains(processName) && i > processes.Length)
                        {
                            UnityEngine.Debug.Log(processName + "没有运行");
                            applicationName = null;
                            isRunning = false;
                        }
                    }
                }
                catch (Exception ep)
                {
                }
            }
            return isRunning;
        }
    }
}