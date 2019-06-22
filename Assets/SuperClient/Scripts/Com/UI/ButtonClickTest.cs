using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//============================================================
//@author	清歌与浊酒
//@create	7/11/2018
//
//@description:
//============================================================
namespace Com.UI
{
    [RequireComponent(typeof(UnityEngine.EventSystems.EventTrigger))]
    public class ButtonClickTest : MonoBehaviour {

        void Start()
        {
            Button btn = this.GetComponent<Button>();
            EventTrigger trigger = btn.gameObject.GetComponent<EventTrigger>();
            EventTrigger.Entry entry1 = new EventTrigger.Entry();
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            EventTrigger.Entry entry3 = new EventTrigger.Entry();

            // 鼠标点击事件
            entry1.eventID = EventTriggerType.PointerClick;
            // 鼠标进入事件 
            entry2.eventID = EventTriggerType.PointerEnter;
            // 鼠标滑出事件 
            entry3.eventID = EventTriggerType.PointerExit;

            entry1.callback = new EventTrigger.TriggerEvent();
            entry1.callback.AddListener(M);
            trigger.triggers.Add(entry1);

            entry2.callback = new EventTrigger.TriggerEvent();
            entry2.callback.AddListener(N);
            trigger.triggers.Add(entry2);

            entry3.callback = new EventTrigger.TriggerEvent();
            entry3.callback.AddListener(F);
            trigger.triggers.Add(entry3);

        }
        private void M(BaseEventData pointData)
        {
            Debug.Log("鼠标点击了!");
        }
        private void N(BaseEventData pointData)
        {
            Debug.Log("鼠标进入了!");
        }
        private void F(BaseEventData pointData)
        {
            Debug.Log("鼠标滑出了!");
        }
    }
}