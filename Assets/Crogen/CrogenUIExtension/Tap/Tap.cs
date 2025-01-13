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
        
        private int _currentIndex;

        public int CurrentIndex
        {
            get=>_currentIndex;
            private set
            {
                _currentIndex = value;
                _currentIndex = Mathf.Clamp(_currentIndex, 0, TapSectionList.Count - 1);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < TapSectionList.Count; i++)
            {
                var tapSectionData = TapSectionList[i];
                
                tapSectionData.isConnected = true;
                var copyValue = i;
                tapSectionData.button.onClick.AddListener(() =>
                {
                    CurrentIndex = copyValue;
                    HandleSelectionMove(tapSectionData.button, tapSectionData.panel);
                });
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
            button.onClick.AddListener(() => { HandleSelectionMove(button, panel); });
        }

        private void HandleSelectionMove(Button button, UIBehaviour panel)
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
        }

        public void MoveToRight()
        {
            ++CurrentIndex;
            HandleSelectionMove(TapSectionList[CurrentIndex].button, TapSectionList[CurrentIndex].panel);
        }

        public void MoveToLeft()
        {
            --CurrentIndex;
            HandleSelectionMove(TapSectionList[CurrentIndex].button, TapSectionList[CurrentIndex].panel);
        }
    }
}
