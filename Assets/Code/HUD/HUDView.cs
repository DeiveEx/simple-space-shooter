using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDView : MonoBehaviour
{
    [SerializeField] private TMP_Text _pointsText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private Button _restartButton;

    public Button RestartButton => _restartButton;

    public void UpdatePoints(int points)
    {
        _pointsText.text = $"Points: {points}";
    }

    public void UpdateHealth(int health)
    {
        _healthText.text = $"Health: {health}";
    }
}
