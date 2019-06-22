using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
//============================================================
//@author	清歌与浊酒
//@create	7/11/2018
//
//@description:
//============================================================
namespace Com.UI
{
	public class ScButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler
    {
        public Sprite SpriteExit;
        public Sprite SpriteEnter;      
        public Sprite SpriteUp;
        public GameObject[] Init;
        private Image style;
        
        private void Awake()
        {
            if (transform.Find("style").GetComponent<Image>()==null)
            {
               transform.Find("style").transform.gameObject.AddComponent<Image>();

            }
            style= transform.Find("style").transform.gameObject.GetComponent <Image>();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (style.sprite == SpriteUp)
            {

                style.sprite = SpriteUp;

            }
            else { style.sprite = SpriteEnter; }
           
        }

        public void OnPointerExit(PointerEventData eventData)
        {if (style.sprite ==SpriteUp)
            {

                style.sprite = SpriteUp;

            }
            else { style.sprite = SpriteExit; }
          
           
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            for (int i = 0; i < Init.Length; i++)
            {
                Init[i].GetComponent<ScButton>().renewal();
            }
            style.sprite = SpriteUp;
        }

         public   void renewal()
          {
            style.sprite = SpriteExit;

        }


    }
}