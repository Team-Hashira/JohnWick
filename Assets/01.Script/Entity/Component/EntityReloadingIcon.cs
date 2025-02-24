using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityReloadingIcon : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent, IEntityDisposeComponent
    {
        private readonly static int _FillAmountShaderHash = Shader.PropertyToID("_FillAmount");

        [SerializeField] private Transform _visualTrm;
        [SerializeField] private SpriteRenderer _charging;
        
        private Material _chargingMat;

        private Entity _entity;
        private float _maxTime;

        public void AfterInit()
        {
            //재장전 시간받는거 구독하기
        }

        public void Dispose()
        {
            //재장전 시간받는거 구독풀기
        }

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _chargingMat = _charging.material;
            _chargingMat.SetFloat(_FillAmountShaderHash, 0);
            _visualTrm.gameObject.SetActive(false);
        }

        private void HandleReloadEvent(float time)
        {
            if (time == 0)
            {
                //End
                _visualTrm.gameObject.SetActive(false);
                _maxTime = 0;
            }
            else if (_maxTime < time)
            {
                //Start
                _visualTrm.gameObject.SetActive(true);
                _maxTime = time;
            }

            _chargingMat.SetFloat(_FillAmountShaderHash, (_maxTime - time) / _maxTime);
        }
    }
}
