using TMPro;
using UnityEngine;

public class HUDView : MonoBehaviour
{
    [SerializeField] private TMP_Text _pointsText;
    [SerializeField] private TMP_Text _healthText;

    public void UpdatePoints(int points)
    {
        _pointsText.text = $"Points: {points}";
    }

    public void UpdateHealth(int health)
    {
        _healthText.text = $"Health: {health}";
    }
}
