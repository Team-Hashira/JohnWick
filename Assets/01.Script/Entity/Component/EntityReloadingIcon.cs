using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityReloadingIcon : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent
    {
        private readonly static int _FillAmountShaderHash = Shader.PropertyToID("_FillAmount");

        [SerializeField] private Transform _visualTrm;
        [SerializeField] private SpriteRenderer _charging;
        
        private EntityWeapon _entityWeapon;
        private Material _chargingMat;

        private Entity _entity;
        private float _maxTime;

        public void AfterInit()
        {
            _entityWeapon = _entity.GetEntityComponent<EntityWeapon>();
        }

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _chargingMat = _charging.material;
            _chargingMat.SetFloat(_FillAmountShaderHash, 0);
        }

        private void HandleReloadEvent(float time)
        {
            if (time == 0)
            {
                //End
                _visualTrm.gameObject.SetActive(false);
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