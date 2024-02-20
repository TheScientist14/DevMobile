using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


