using DG.Tweening;
using Hashira.Core;
using Hashira.Pathfind;
using Unity.Cinemachine;
using UnityEngine;

namespace Hashira.MainScreen
{
    public class MainScreenEffect : MonoBehaviour
    {
        private static Material _mainScreenMat;
        private static Transform _levelTransform;
        private static Transform _transform;
        private static CinemachineCamera _cinemachineCamera;

        public static Transform GetLevelTransform() => _levelTransform;
        public static Transform GetTransform() => _transform;

        private static Vector2 originPos;
        private static Vector2 viewportMin;
        private static Vector2 viewportMax;

        // SimpeShockWave
        private static readonly int _simpleShckWave_ValueID = Shader.PropertyToID("_SimpleShckWave_Value");
        private static readonly int _playerPos = Shader.PropertyToID("_PlayerPos");

        // WallShockWave
        private static readonly int _wallShockWave_ValueID = Shader.PropertyToID("_WallShockWave_Value");
        private static readonly int _wallShockWave_StrengthID = Shader.PropertyToID("_WallShockWave_Strength");
        private static readonly int _wallShockWave_MaxID = Shader.PropertyToID("_WallShockWave_Max");
        private static readonly int _wallShockWave_MinID = Shader.PropertyToID("_WallShockWave_Min");

        // Tweens
        private static Tween _moveTween;
        private static Tween _rotateTween;
        private static Tween _shakeTween;
        private static Tween _reverseXTween;
        private static Tween _reverseYTween;
        private static Tween _scalingTween;

        private void Awake()
        {
            _levelTransform = FindFirstObjectByType<Stage.Stage>().transform;
            _transform = this.transform;
            _mainScreenMat = GetComponent<MeshRenderer>().material;
            _cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();

            Vector2 stageScale = _transform.localScale;
            viewportMin = Camera.main.ViewportToWorldPoint(Vector2.zero);
            viewportMin += stageScale;
            viewportMax = Camera.main.ViewportToWorldPoint(Vector2.one);
            viewportMax -= stageScale;
        }
        private void Update()
        {
            var playerPos = PlayerManager.Instance.Player.transform.position;

            if (Input.GetKey(KeyCode.Alpha0))
            {
                OnRotate(0);
            }
            if (Input.GetKey(KeyCode.Alpha1))
            {
                OnRotate(35);
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                OnRotate(180);
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {
                OnScaling();
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                OnScaling(0.5f);
            }
            if (Input.GetKey(KeyCode.Alpha5))
            {
                OnScaling(0.25f);
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

            if (Input.GetKeyDown(KeyCode.Equals))
                OnReverseX();

            if (Input.GetKeyDown(KeyCode.Minus))
                OnReverseY();
        }

        private void SetPlayerPos(Vector2 pos) => _mainScreenMat.SetVector(_playerPos, pos);

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

        public static void OnRotate(float value)
        {
            _rotateTween?.Kill();
            _rotateTween = _cinemachineCamera.transform.DORotate(new Vector3(0, 0, -value), 0.25f).SetEase(Ease.OutBounce);
        }

        /// <summary>
        /// Only Use Up, Right, Down, Left, Zero
        /// </summary>
        /// <param name="directionType"></param>
        public static void OnLocalMoveScreenSide(DirectionType directionType)
        {
            Vector2 dir = (Vector2)Direction2D.GetIntDirection(directionType) * float.MaxValue;

            Vector2 curViewportPos = Camera.main.WorldToViewportPoint(-Camera.main.transform.position);
            Vector2 finalViewport = curViewportPos;

            finalViewport = new Vector2(
                Mathf.Clamp01(curViewportPos.x + dir.x),
                Mathf.Clamp01(curViewportPos.y + dir.y));

            OnMoveScreenSide(finalViewport);
        }
        public static void OnLocalMoveScreenSide(Vector2 direction)
        {
            direction = direction.normalized * float.MaxValue;

            Vector2 curViewportPos = Camera.main.WorldToViewportPoint(_transform.position);
            Vector2 finalViewport = curViewportPos;

            finalViewport = new Vector2(
                Mathf.Clamp01(curViewportPos.x + direction.x),
                Mathf.Clamp01(curViewportPos.y + direction.y));

            OnMoveScreenSide(finalViewport);
        }
        public static void OnMoveScreenSide(Vector2 viewPort)
        {
            Vector2 worldPos = Camera.main.ViewportToWorldPoint(viewPort);

            worldPos = new Vector2(
                Mathf.Clamp(worldPos.x, viewportMin.x, viewportMax.x),
                Mathf.Clamp(worldPos.y, viewportMin.y, viewportMax.y));

            OnMove(worldPos);
        }
        public static void OnMove(Vector2 pos)
        {
            _moveTween?.Kill();

            Vector3 cameraPos = -(Vector3)pos;
            cameraPos.z = _cinemachineCamera.transform.position.z;
            _moveTween = _cinemachineCamera.transform.DOMove(cameraPos, 0.25f).SetEase(Ease.OutBounce)
                .OnComplete(()=>originPos = _transform.position);
        }

        public static void OnShake(float strength, int vibrato, float time)
        {
            _shakeTween?.Kill();
            _shakeTween = _transform.DOShakePosition(time, strength, vibrato)
                .OnComplete(() => _transform.position = originPos);
        }

        public static void OnScaling(float scale = 1)
        {
            _scalingTween?.Kill();
            _scalingTween = DOTween.To(() => _cinemachineCamera.Lens.OrthographicSize, x => _cinemachineCamera.Lens.OrthographicSize = x, 10 * (1f/scale), 0.25f).SetEase(Ease.OutBounce);
        }

        public static void OnReverseX()
        {
            _reverseXTween?.Kill();
            _reverseXTween = _transform.DOScaleX(_transform.localScale.x * -1, 0.25f);
        }
        public static void OnReverseY()
        {
            _reverseYTween?.Kill();
            _reverseYTween = _transform.DOScaleY(_transform.localScale.y * -1, 0.25f);
        }
    }
}
