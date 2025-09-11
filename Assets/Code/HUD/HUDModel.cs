public class HUDModel
{
    public HealthComponent PlayerHealth;
    public ScoreComponent ScoreController;

    public HUDModel()
    {
        var player = SimpleServiceLocator.GetService<PlayerController>();
        
        PlayerHealth = player.Health;
        ScoreController = player.Score;
    }
}
