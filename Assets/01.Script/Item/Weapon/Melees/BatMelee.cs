using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities.Components;
using Hashira.Entities;
using System;
using UnityEngine;
using DG.Tweening;
using Hashira.Items.SubItems;

namespace Hashira.Items.Weapons
{
    public class BatMelee : SubItem
    {
        private int _damage;

        public event Action<int> OnAttackEvent;

        private Sequence animationSeq;

        private bool _onParrying;
        private bool _isCharged;

        public override void Equip(EntityItemHolder entityItemHolder)
        {
            base.Equip(entityItemHolder);
            //EntityMeleeWeapon.DamageCaster.OnDamageCastSuccessEvent += HandleDamageCastSuccessEvent;
        }

        //private void HandleDamageCastSuccessEvent(RaycastHit2D hit)
        //{
        //    CameraManager.Instance.ShakeCamera(10, 10, 0.2f);
        //}

        public override void Use()
        {
            base.Use();
            //Vector3 startRot = Vector3.forward * SubItemSO.RotateMax;
            //Vector3 endRot = Vector3.forward * SubItemSO.RotateMin;
            //float duration = SubItemSO.AttackDuration;
            //float afterDelay = SubItemSO.AttackAfterDelay;

            //_onParrying = true;
            //_isCharged = EntityMeleeWeapon.IsCharged;

            //(EntityMeleeWeapon.DamageCaster as BoxDamageCaster2D).size = SubItemSO.AttackRangeSize;
            //(EntityMeleeWeapon.DamageCaster as BoxDamageCaster2D).center = SubItemSO.AttackRangeOffset;

            //Vector2 attackDir = EntityMeleeWeapon.transform.right;

            //if (animationSeq != null && animationSeq.IsActive()) animationSeq.Kill();
            //animationSeq = DOTween.Sequence();
            //animationSeq.AppendCallback(() => EntityMeleeWeapon.transform.localEulerAngles = startRot);
            //animationSeq.Append(EntityMeleeWeapon.transform.DOLocalRotate(endRot, duration).SetEase(Ease.OutBack));
            //animationSeq.InsertCallback(duration / 5, () =>
            //{
            //    EntityMeleeWeapon.DamageCaster.CastDamage(damage, knockback: attackDir * 15f);

            //    //휘두르는 이펙트
            //    Vector3 effectPos = EntityMeleeWeapon.Entity.transform.position + EntityMeleeWeapon.transform.right * 0.85f + Vector3.up * 0.2f;
            //    GameObject effectObj = EntityMeleeWeapon.transform.gameObject.Pop(EffectPoolType.MeleeAttackEffect, effectPos, Quaternion.identity).gameObject;
            //    effectObj.transform.localScale = new Vector3(Mathf.Sign(EntityMeleeWeapon.transform.right.x), 1, 1);
            //});
            //animationSeq.InsertCallback(duration / 2, () =>
            //{
            //    _onParrying = false;
            //    _isCharged = false;
            //});
            //animationSeq.AppendInterval(afterDelay);
            //animationSeq.AppendCallback(() => AttackEnd());
        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();
            //if (_onParrying)
            //{
            //    RaycastHit2D[] hits = EntityMeleeWeapon.DamageCaster.CastOverlap();
            //    foreach (var hit in hits)
            //    {
            //        Transform owner = EntityMeleeWeapon.Entity.transform;
            //        if (hit.transform.TryGetComponent(out IParryingable parryingable) && parryingable.Owner != owner)
            //            parryingable.Parrying(WhatIsTarget, owner, _isCharged);
            //    }
            //}
        }

        //private void AttackEnd()
        //{
        //    EntityGunHolder entityGunWeapon = EntityMeleeWeapon.GunWaepon;
        //    entityGunWeapon.IsMeleeWeaponMode = false;
        //    entityGunWeapon.EquipWeapon(entityGunWeapon.CurrentWeapon, entityGunWeapon.CurrentIndex);
        //}

        public override void UnEquip()
        {
            base.UnEquip();
            //EntityMeleeWeapon.DamageCaster.OnDamageCastSuccessEvent -= HandleDamageCastSuccessEvent;
            //EntityMeleeWeapon = null;
        }
    }
}