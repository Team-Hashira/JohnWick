#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Hashira.Items.Weapons.Editor
{
    public static class PartsPositionViewer
    {
        private static readonly Rect SlotRect = new(0, 0, 420, 270);
        private static readonly Vector2 PartSlotSize = new(80, 80);

        public static void Draw(WeaponSO weaponSO)
        {
            float inspectorWidth = EditorGUIUtility.currentViewWidth;
            float widthOffset = 25f;
            Rect rectResize = new Rect(0, 0, 0, SlotRect.height/SlotRect.width * (inspectorWidth-widthOffset));
            
            GUILayout.Box(new GUIContent(weaponSO.itemSprite.texture), GUI.skin.window, GUILayout.Height(rectResize.height));
            Rect spaceRect = GUILayoutUtility.GetRect(new GUIContent("Preview"), GUI.skin.window, GUILayout.Height(rectResize.height));
            Vector2 partSlotResize = PartSlotSize * (inspectorWidth/SlotRect.width);
            
            foreach (var pair in weaponSO.partsEquipPosDict)
            {
                Vector2 partSlotReposition = new Vector2(pair.Value.x, -pair.Value.y);

                partSlotReposition += new Vector2
                (
                    inspectorWidth / 2,
                    spaceRect.position.y - inspectorWidth / 2
                );

                partSlotReposition += new Vector2(-partSlotResize.x/2, partSlotResize.y/2);
                
                GUI.color = Color.cyan;
                {
                    GUI.Box(new Rect(partSlotReposition, 
                        partSlotResize), new GUIContent(pair.Key.ToString()), GUI.skin.window);
                }
                GUI.color = Color.white;
            }
        }
    }
    
    [CustomEditor(typeof(GunSO))]
    public class WeaponSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Label("==========Parts Position Preview=========");
            PartsPositionViewer.Draw(target as GunSO);
        }
    }
}
#endif