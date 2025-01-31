#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Crogen.CrogenEditorExtension.Editor
{
    public class EditorTextureExtension
    {
        public static Texture2D ConvertToTexture2D(Sprite sprite, FilterMode filterMode = FilterMode.Bilinear)
        {
            Texture2D result = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] colors = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
            result.SetPixels(colors);
            result.filterMode = filterMode;
            result.Apply();
            
            return result;
        }
    }
}
#endif