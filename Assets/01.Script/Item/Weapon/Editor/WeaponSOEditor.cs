#if UNITY_EDITOR
using Crogen.CrogenEditorExtension.Editor;
using UnityEditor;
using UnityEngine;

namespace Hashira.Items.Weapons.Editor
{
    public static class PartsPositionViewer
    {
        private static string PartSlotPrefabPath = "Assets\\03.Prefab\\UI\\PartSlot.prefab";
        private static string WeaponSlotPrefabPath = "Assets\\03.Prefab\\UI\\WeaponSlot.prefab";
		private static float WidthOffset = 34.79999f;

		public static void Draw(GunSO gunSO)
        {
			if (gunSO.itemIcon == null) return;

			// PrefabLoad
			GameObject partSlotObject = AssetDatabase.LoadAssetAtPath<GameObject>(PartSlotPrefabPath);
			Debug.Assert(partSlotObject != null, "Part slot prefab is null!");
			Vector2 partSlotSize = (partSlotObject.transform as RectTransform).sizeDelta;

			GameObject waeponSlotObject = AssetDatabase.LoadAssetAtPath<GameObject>(WeaponSlotPrefabPath);
			Debug.Assert(waeponSlotObject != null, "Part slot prefab is null!");
			Vector2 weaponSlotSize = (waeponSlotObject.transform as RectTransform).sizeDelta;

			// InspectorValues
			float inspectorWidth = EditorGUIUtility.currentViewWidth;
            float mul = inspectorWidth / weaponSlotSize.x;

            // BackgroundSize
            Rect rectResize = new Rect(0, 0, inspectorWidth-WidthOffset, weaponSlotSize.y/ weaponSlotSize.x * (inspectorWidth-WidthOffset));

            // WeaponDraw
            Texture2D texture = EditorTextureExtension.ConvertToTexture2D(gunSO.itemIcon, FilterMode.Point);
            GUILayout.Box(GUIContent.none, GUI.skin.window, GUILayout.Height(rectResize.height));
            Rect spaceRect = GUILayoutUtility.GetRect(new GUIContent("Preview"), GUI.skin.window, GUILayout.Height(rectResize.height));
            GUI.DrawTexture(new Rect(spaceRect.x, spaceRect.y-spaceRect.height, spaceRect.width, spaceRect.height), texture, ScaleMode.ScaleToFit);

			// PartSlotDraw
			Vector2 partSlotResize = partSlotSize * mul;
            foreach (var pair in gunSO.partsEquipUIPosDict)
            {
                Vector2 partSlotReposition = new Vector2(pair.Value.x, -pair.Value.y);

				partSlotReposition = new Vector2(rectResize.width/2 * partSlotReposition.x, rectResize.height/2 * partSlotReposition.y);
				partSlotReposition -= new Vector2(partSlotResize.x/2, partSlotResize.y/2);
                partSlotReposition += spaceRect.position - new Vector2(-spaceRect.size.x, spaceRect.size.y)/2;

				GUI.color = Color.cyan;
                {
                    GUI.Box(new Rect(partSlotReposition, partSlotResize), new GUIContent(pair.Key.ToString()), GUI.skin.window);
                }
                GUI.color = Color.white;
            }
        }
    }
    
    [CustomEditor(typeof(GunSO))]
    public class GunSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Label("==========Parts Position Preview=========");
            PartsPositionViewer.Draw(target as GunSO);
        }
    }
    
    //이부분 필요 없을듯
    [CustomEditor(typeof(MeleeSO))]
    public class MeleeSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
#endif
