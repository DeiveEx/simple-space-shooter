using System;

[Serializable]
public class GameSettings
{
    //Overall
    public float RespawnDelay = 1;
    public int InitialAsteroidAmount = 5;
    
    //Player
    public int PlayerHealth = 3;
    
    //Ship
    public int ShipHealth = 3;
    public int ShipSpeed = 3;
    public int ShipFireRate = 2;
}