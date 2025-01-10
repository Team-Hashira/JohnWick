#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Crogen.UIExtension.Editor
{
    [CustomEditor(typeof(Tap))]
    public class TapEditor : UnityEditor.Editor
    {
        private Tap _tap;
        private TapSectionData _currentSelectedSection;
        private static Button _buttonPrefab;
        private static UIBehaviour _panelPrefab;
        
        [MenuItem("GameObject/UI/Tap")]
        static void CreateTapObject()
        {
            var tapPrefab = AssetDatabase.LoadAssetAtPath<Tap>("Assets/Crogen/CrogenUIExtension/Tap/Tap.prefab");
            var parentCanvas = Selection.activeTransform?.GetComponent<Canvas>();
            if(parentCanvas == null)
                parentCanvas = FindFirstObjectByType<Canvas>();
                
            var tap = Instantiate(tapPrefab, parentCanvas.transform);
            tap.name = "Tap";
        }
        
        private void OnEnable()
        {
            _buttonPrefab = AssetDatabase.LoadAssetAtPath<Button>("Assets/Crogen/CrogenUIExtension/Tap/Button.prefab");
            _panelPrefab = AssetDatabase.LoadAssetAtPath<UIBehaviour>("Assets/Crogen/CrogenUIExtension/Tap/Panel.prefab");
            
            _tap = (Tap)target;
            if (_tap.TapSectionList == null || _tap.TapSectionList.Count == 0) return;
            _currentSelectedSection = _tap.TapSectionList[0];
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (_tap.TapSectionList == null) return;
            
            GUILayout.BeginHorizontal(GUI.skin.box);
            {
                if (GUILayout.Button("+"))
                {
                    Button button = Instantiate(_buttonPrefab, _tap.buttonGroup);
                    button.gameObject.name = $"Button{(_tap.TapSectionList.Count+1):00}";
                    button.transform.GetComponentInChildren<TextMeshProUGUI>().text = $"Section{(_tap.TapSectionList.Count+1):00}";
                    
                    UIBehaviour panel = Instantiate(_panelPrefab, _tap.panelGroup);    
                    panel.gameObject.name = $"Panel{(_tap.TapSectionList.Count+1):00}";
                    _tap.TapSectionList.Add(new TapSectionData {button = button, panel = panel});
                    EditorUtility.SetDirty(_tap);
                }

                if (_tap.TapSectionList != null && _tap.TapSectionList.Count != 0)
                {
                    if (GUILayout.Button("-"))
                    {
                    
                        var lastSection = _tap.TapSectionList[^1];
                        DestroyImmediate(lastSection.button.gameObject);
                        DestroyImmediate(lastSection.panel.gameObject);
                        _tap.TapSectionList.Remove(lastSection);
                        EditorUtility.SetDirty(_tap);
                    }    
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif