using UnityEngine;

public interface IEnemy
{
    int damage { get; set; }
    bool isAlive { get; set; }
    void TakeDamage(int dmg);
}