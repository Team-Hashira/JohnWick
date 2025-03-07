using DG.Tweening;
using Hashira.Core;
using Hashira.Pathfind;
using UnityEngine;

namespace Hashira.MainScreen
{
    public class MainScreenEffect : MonoBehaviour
    {
        private static Material _mainScreenMat;
        private static Transform _levelTransform;
        private static Transform _transform;

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

        // Tweens
        private static Tween _moveTween;
        private static Tween _rotateTween;
        private static Tween _shakeTween;

        private void Awake()
        {
            _levelTransform = FindFirstObjectByType<Stage.Stage>().transform;
            _transform = this.transform;
            _mainScreenMat = GetComponent<MeshRenderer>().material;
        }
        private void Update()
        {
            var playerPos = PlayerManager.Instance.Player.transform.position;

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

            if (Input.GetKeyDown(KeyCode.UpArrow))
                OnLocalMoveScreenSide(DirectionType.Up);

            if (Input.GetKeyDown(KeyCode.DownArrow))
                OnLocalMoveScreenSide(DirectionType.Down);

            if (Input.GetKeyDown(KeyCode.RightArrow))
                OnLocalMoveScreenSide(DirectionType.Right);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                OnLocalMoveScreenSide(DirectionType.Left);

            if (Input.GetKeyDown(KeyCode.Backspace))
                OnLocalMoveScreenSide(DirectionType.Zero);
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
            Vector2 abs = new Vector2(Mathf.Abs(pos.x), Mathf.Abs(pos.y));
            Vector2 normal = new Vector2(Mathf.Sign(pos.x), Mathf.Sign(pos.y)) * 0.5f + Vector2.one * 0.5f;

            Vector2 max = new Vector2(abs.x > abs.y ? normal.x : 0, abs.x < abs.y ? normal.y : 0);
            Vector2 min = new Vector2(
                abs.x > abs.y ? normal.x > 0.5f ? normal.x - 0.1f : normal.x + 0.1f : 0,
                abs.x < abs.y ? normal.y > 0.5f ? normal.y - 0.1f : normal.y + 0.1f : 0);

            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                _mainScreenMat.SetVector(_wallShockWave_MaxID, max);
                _mainScreenMat.SetVector(_wallShockWave_MinID, min);

                _mainScreenMat.SetFloat(_wallShockWave_StrengthID, 0.5f);
                _mainScreenMat.SetFloat(_wallShockWave_ValueID, 0);

            });
            seq.Append(_mainScreenMat.DOFloat(1, _wallShockWave_ValueID, 1.75f));
            seq.Append(_mainScreenMat.DOFloat(0, _wallShockWave_StrengthID, 0.1f));
        }

        public static void OnRotateEffect(float value)
        {
            _rotateTween?.Kill();
            _rotateTween = _levelTransform.DORotate(new Vector3(0, 0, value), 0.25f).SetEase(Ease.OutBounce);
        }

        public static void OnMoveScreenSide(Vector2 viewPort)
        {
            Vector2 worldPos = Camera.main.ViewportToWorldPoint(viewPort);

            Vector2 stageScale = _transform.localScale;

            Vector2 min = Camera.main.ViewportToWorldPoint(Vector2.zero);
            min += stageScale;
            Vector2 max = Camera.main.ViewportToWorldPoint(Vector2.one);
            max -= stageScale;

            worldPos = new Vector2(
                Mathf.Clamp(worldPos.x, min.x, max.x), 
                Mathf.Clamp(worldPos.y, min.y, max.y));

            _moveTween?.Kill();
            _moveTween = _levelTransform.DOMove(worldPos, 0.25f).SetEase(Ease.OutBounce);
        }

        /// <summary>
        /// Only Use Up, Right, Down, Left, Zero
        /// </summary>
        /// <param name="directionType"></param>
        public static void OnLocalMoveScreenSide(DirectionType directionType)
        {
            Vector2 curViewportPos = Camera.main.WorldToViewportPoint(_levelTransform.position);

            Vector2 finalViewport = curViewportPos;

            Vector2 dir = Direction2D.GetIntDirection(directionType);

            finalViewport = new Vector2(
                Mathf.Clamp01(curViewportPos.x + dir.x),
                Mathf.Clamp01(curViewportPos.y + dir.y));

            OnMoveScreenSide(finalViewport);
        }

        public static void OnMoveEffect(Vector2 pos)
        {
            _levelTransform.DOMove(pos, 0.25f).SetEase(Ease.OutBounce);
        }

        public static void OnShake(float strength, int vibrato, float time)
        {
            _shakeTween?.Kill();
            _shakeTween = _transform.DOShakePosition(time, strength, vibrato).OnComplete(() => _transform.position = Vector3.zero);
        }
    }
}
