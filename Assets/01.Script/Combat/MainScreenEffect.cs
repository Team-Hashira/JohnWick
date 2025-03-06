using DG.Tweening;
using UnityEngine;

namespace Hashira.MainScreen
{
    public class MainScreenEffect : MonoBehaviour
    {
        private static Material _mainScreenMat;

        // SimpeShockWave
        private static readonly int _simpleShckWave_ValueID = Shader.PropertyToID("_SimpleShckWave_Value");
        private static readonly int _playerPos = Shader.PropertyToID("_PlayerPos");

        // WallShockWave
        private static readonly int _wallShockWave_ValueID = Shader.PropertyToID("_WallShockWave_Value");
        private static readonly int _wallShockWave_StrengthID = Shader.PropertyToID("_WallShockWave_Strength");
        private static readonly int _wallShockWave_MaxID = Shader.PropertyToID("_WallShockWave_Max");
        private static readonly int _wallShockWave_MinID = Shader.PropertyToID("_WallShockWave_Min");

        // Rotate
        private static readonly int _rotate_value = Shader.PropertyToID("_Rotate_Value");

        private void Awake()
        {
            _mainScreenMat = GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            var playerPos = GameManager.Instance.Player.transform.position;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnShockWaveEffect(playerPos);
            }

            if (Input.GetKey(KeyCode.Alpha0))
            {
                OnRotateEffect(0);
            }

            if (Input.GetKey(KeyCode.Alpha1))
            {
                OnRotateEffect(35);
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                OnRotateEffect(180);
            }
        }

        public void SetPlayerPos(Vector2 pos) => _mainScreenMat.SetVector(_playerPos, pos);

        public void OnShockWaveEffect(Vector2 pos)
        {
            SetPlayerPos(pos);
            _mainScreenMat.DOKill();
            _mainScreenMat.SetFloat(_simpleShckWave_ValueID, 0);
            _mainScreenMat.DOFloat(4, _simpleShckWave_ValueID, 1f);
        }

        public static void OnWallShockWaveEffect(Vector2 pos)
        {
            Debug.Log("dfdf");
            Vector2 abs = new Vector2(Mathf.Abs(pos.x), Mathf.Abs(pos.y));
            Vector2 normal = new Vector2(Mathf.Sign(pos.x), Mathf.Sign(pos.y)) * 0.5f + Vector2.one * 0.5f;

            Vector2 max = new Vector2(abs.x > abs.y ? normal.x : 0, abs.x < abs.y ? normal.y : 0);
            Vector2 min = new Vector2(
                abs.x > abs.y ? normal.x > 0.5f ? normal.x - 0.1f : normal.x + 0.1f : 0,
                abs.x < abs.y ? normal.y > 0.5f ? normal.y - 0.1f : normal.y + 0.1f : 0);

            _mainScreenMat.SetVector(_wallShockWave_MaxID, max);
            _mainScreenMat.SetVector(_wallShockWave_MinID, min);

            _mainScreenMat.SetFloat(_wallShockWave_StrengthID, 1);
            _mainScreenMat.SetFloat(_wallShockWave_ValueID, 0);

            Sequence seq = DOTween.Sequence();
            seq.Append(_mainScreenMat.DOFloat(1, _wallShockWave_ValueID, 1.75f));
            seq.Append(_mainScreenMat.DOFloat(0, _wallShockWave_StrengthID, 0.1f));
        }

        public void OnRotateEffect(float value)
        {
            _mainScreenMat.DOFloat(value, _rotate_value, 0.25f).SetEase(Ease.OutBounce);
        }
    }
}
