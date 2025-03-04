using UnityEditor;
using UnityEngine;

namespace Hashira.Core
{
    public class KeywordReplace : UnityEditor.AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            // path에 .이 포함되어 있다면 해당 index를 받아옴
            int index = path.LastIndexOf(".");
            // 그렇지않다면 -1이 index로 들어오므로 return
            if (index < 0)
                return;

            // .이 포함된 index로 path를 나눠서 확장자가 .cs가 아니면 return
            string file = path.Substring(index);
            if (file != ".cs")
                return;

            // 유니티 에셋 데이터베이스 상의 주소를 실제 주소로 변경
            index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;

            if (!System.IO.File.Exists(path))
                return;

            // 스크립트 내용을 메모리에 불러오기
            string fileContent = System.IO.File.ReadAllText(path);

            // 키워드 대체
            fileContent = fileContent.Replace("#DATE#", System.DateTime.Now.ToString("yyyy년 MM월 dd일 tt h시 mm분", System.Globalization.CultureInfo.CreateSpecificCulture("ko-KR")));
            DeveloperInformation information = AssetDatabase.LoadAssetAtPath<DeveloperInformation>("Assets/Editor/DeveloperInformation.asset");
            if (information)
            {
                fileContent = fileContent.Replace("#AUTHOR#", information.Name);
            }
            else
            {
                Debug.LogError("Failed to load DeveloperInformation");
            }

            // 대체가 끝나면 다시 파일에 쓰기
            System.IO.File.WriteAllText(path, fileContent);

            // 다하고 나면 꼭 호출
            AssetDatabase.Refresh();
        }
    }

    [CreateAssetMenu(fileName = "DeveloperInformation", menuName = "DeveloperInformation", order = 0)]
    public class DeveloperInformation : ScriptableObject
    {
        [SerializeField]
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
        }
    }
}
