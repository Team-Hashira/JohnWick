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
            float mul = inspectorWidth / SlotRect.width;
            float widthOffset = 25f;
            Rect rectResize = new Rect(0, 0, 0, SlotRect.height/SlotRect.width * (inspectorWidth-widthOffset));

            var originTexture = weaponSO.itemSprite.texture;
            Rect spriteRect = weaponSO.itemSprite.rect;

            Texture2D croppedTexture = new Texture2D((int)spriteRect.width, (int)spriteRect.height);
            croppedTexture.SetPixels(originTexture.GetPixels(
                (int)spriteRect.x,
                (int)spriteRect.y,
                (int)spriteRect.width,
                (int)spriteRect.height));
            croppedTexture.Apply();
            croppedTexture.filterMode = FilterMode.Point;
            GUILayout.Box(GUIContent.none, GUI.skin.window, GUILayout.Height(rectResize.height));
            Rect spaceRect = GUILayoutUtility.GetRect(new GUIContent("Preview"), GUI.skin.window, GUILayout.Height(rectResize.height));
            GUI.DrawTexture(new Rect(spaceRect.x, spaceRect.y-spaceRect.height, spaceRect.width, spaceRect.height), croppedTexture, ScaleMode.ScaleToFit);
            
            Vector2 partSlotResize = PartSlotSize * mul;
            
            foreach (var pair in weaponSO.partsEquipPosDict)
            {
                Vector2 partSlotReposition = new Vector2(pair.Value.x, -pair.Value.y) * mul;

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
