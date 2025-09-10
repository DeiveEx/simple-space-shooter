public class HUDModel
{
    public HealthComponent PlayerHealth;
    public ScoreController ScoreController;

    public HUDModel()
    {
        PlayerHealth = GameManager.Instance.PlayerShip.GetComponent<HealthComponent>();
        ScoreController = GameManager.Instance.PlayerShip.GetComponent<ScoreController>();
    }
}
