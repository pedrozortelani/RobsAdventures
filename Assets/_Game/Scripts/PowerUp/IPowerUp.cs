using UnityEngine;

public interface IPowerUp
{
    void UsePowerUP(PlayerController player);
    void DeactivatePowerUP(PlayerController player);
    void PickPowerUP(PlayerController player);
    void LosePowerUP(PlayerController player);
}
