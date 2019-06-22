using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;
//============================================================
//@author	清歌与浊酒
//@create	7/10/2018
//
//@description:
//============================================================
namespace Com.win
{
	public class WindowMod : MonoBehaviour
    {
        public Text timetext;

        private void FixedUpdate()
        {

            FixedTime();
        }
        /// <summary>
        /// 显示时间
        /// </summary>
        private void FixedTime()
        {if (System.DateTime.Now.Minute < 10)
            { timetext.text = System.DateTime.Now.Hour + " : 0" + System.DateTime.Now.Minute; }
            else
            { timetext.text = System.DateTime.Now.Hour + " : " + System.DateTime.Now.Minute; }

        }
        void Awake()
        {

            
        }
    }
}