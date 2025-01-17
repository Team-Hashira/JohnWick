namespace Hashira.Items.Weapons
{
    public class MeleeWeapon : Weapon
    {
        public MeleeSO MeleeSO { get; private set; }
        
        public override void WeaponUpdate()
        {
            base.WeaponUpdate();
        }

        public override void Attack(int damage, bool isDown)
        {
            base.Attack(damage, isDown);
        }
    }
}
