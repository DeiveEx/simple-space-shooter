public class HUDModel
{
    public HealthComponent PlayerHealth;
    public ScoreComponent ScoreController;

    public HUDModel()
    {
        PlayerHealth = GameManager.Instance.Player.Health;
        ScoreController = GameManager.Instance.Player.Score;
    }
}
