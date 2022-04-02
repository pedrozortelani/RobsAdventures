using System;

[Serializable]
public class PlayerData
{
    public DateTime timestamp;

    public int lives;
    public int marbles;
    public int checkpoint;

    public PlayerData(PlayerController player)
    {
        timestamp = DateTime.Now;

        lives = player.lives;
        marbles = player.marbles;
        checkpoint = player.checkpoint;
    }
}
