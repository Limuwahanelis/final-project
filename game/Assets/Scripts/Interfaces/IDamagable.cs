using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IDamagable
{
    void TakeDamage(int dmg);
    void Kill();
    void Knockback();
    void SlowDown(float slowDownFactorx, float slowDownFactory);
}
