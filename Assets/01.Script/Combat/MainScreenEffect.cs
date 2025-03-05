using DG.Tweening;
using UnityEngine;

namespace Hashira.MainScreen
{
    public class MainScreenEffect : MonoBehaviour
    {
        private Material _mainScreenMat;

        // SimpeShockWave
        private readonly int _simpleShckWave_ValueID = Shader.PropertyToID("_SimpleShckWave_Value");
        private readonly int _playerPos = Shader.PropertyToID("_PlayerPos");

        // WallShockWave
        private readonly int _wallShockWave_ValueID = Shader.PropertyToID("_WallShockWave_Value");
        private readonly int _wallShockWave_StrengthID = Shader.PropertyToID("_WallShockWave_Strength");
        private readonly int _wallShockWave_MaxID = Shader.PropertyToID("_WallShockWave_Max");
        private readonly int _wallShockWave_MinID = Shader.PropertyToID("_WallShockWave_Min");


        private void Awake()
        {
            _mainScreenMat = GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("dfdf");
                OnShockWaveEffect(GameManager.Instance.Player.transform.position);
            }
        }

        public void SetPlayerPos(Vector2 pos)
        {
            _mainScreenMat.SetVector(_playerPos, pos);
        }

        public void OnShockWaveEffect(Vector2 pos)
        {
            SetPlayerPos(pos);
            _mainScreenMat.DOKill();
            _mainScreenMat.SetFloat(_simpleShckWave_ValueID, 0);
            _mainScreenMat.DOFloat(4, _simpleShckWave_ValueID, pos.magnitude);
        }

        public void OnWallShockWaveEffect(Vector2 pos)
        {
            _mainScreenMat.SetVector(_wallShockWave_MaxID, );
            _mainScreenMat.SetVector(_wallShockWave_MinID, );
        }
    }
}
