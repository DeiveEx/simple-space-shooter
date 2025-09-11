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
    public int ShipSpeed = 3;
    
    //Gun
    public int GunFireRate = 2;
    public float ProjectileLifetime = 1;
    public float ProjectileSpeed = 1;
}