using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//============================================================
//@author	清歌与浊酒
//@create	7/12/2018
//
//@description:
//============================================================
namespace Com.UI
{
	public class AlertsTip : MonoBehaviour {
        [Header("提示文字")]
        public Text maskBlurimg;
        private void Awake()
        {
           // gameObject.SetActive(false);

        }
        public void TipSettings(string str)
        {

            maskBlurimg.text = str;
        }
        public void Button_esc()
        {

            Application.Quit();
        }
        public void Button_close()
        {
            gameObject.SetActive(false);         
        
        }
    }
}