using UnityEngine;


namespace Proto
{
    public enum DamageCauser
    {
        SpaceShip,
        Bullets,
        Asteroid
    }

    public interface IDamageable
    {
        public void TakeDamage(DamageCauser damageCauser_, float damages_, Vector2 impactPoint_);
    }
}



