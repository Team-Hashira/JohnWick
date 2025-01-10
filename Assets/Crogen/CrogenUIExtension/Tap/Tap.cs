using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Crogen.UIExtension
{
    [Serializable]
    public class TapSectionData
    {
        public Button button;
        public UIBehaviour panel;
        [HideInInspector] public bool isConnected = false;
    }
    
    public class Tap : UIBehaviour
    {
        [field:SerializeField] public List<TapSectionData> TapSectionList {get; private set;}
        public Transform buttonGroup;
        public Transform panelGroup;
        
        protected override void Awake()
        {
            base.Awake();
            foreach (var tapSectionData in TapSectionList)
            {
                tapSectionData.isConnected = true;
                AddListener(tapSectionData.button, tapSectionData.panel);
            }
        }

        protected override void Start()
        {
            base.Start();
            TapSectionList.ForEach(x=>x.panel.gameObject.SetActive(false));
            TapSectionList[0].panel.gameObject.SetActive(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (var tapSectionData in TapSectionList)
                tapSectionData.button.onClick.RemoveAllListeners();
        }

        public void AddSection(Button button, UIBehaviour panel)
        {
            TapSectionList.Add(new TapSectionData {button = button, panel = panel});
            AddListener(button, panel);
        }

        private void AddListener(Button button, UIBehaviour panel)
        {
            button.onClick.AddListener(() =>
            {
                TapSectionList.FirstOrDefault(x=>x.panel.gameObject.activeSelf)?.panel.gameObject.SetActive(false);
                TapSectionList.ForEach(x =>
                {
                    if (x.button == button)
                    {
                        button.image.rectTransform.SetAsFirstSibling();
                    }
                    else
                    {
                        button.image.rectTransform.SetAsLastSibling();
                    }
                });
                panel.gameObject.SetActive(true);   
            });
        }
    }
}
