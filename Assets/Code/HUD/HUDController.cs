using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    [SerializeField] private HUDView _view;
    
    private HUDModel _model;

    private void Start()
    {
        _model = new HUDModel();
        
        _model.PlayerHealth.HealthChanged += () => _view.UpdateHealth(_model.PlayerHealth.CurrentHealth);
        _model.PlayerHealth.Died += () => _view.GameObjectPanel.gameObject.SetActive(true);
        _model.ScoreController.ScoreChanged += () => _view.UpdatePoints(_model.ScoreController.CurrentScore);
        
        _view.UpdateHealth(_model.PlayerHealth.CurrentHealth);
        _view.UpdatePoints(_model.ScoreController.CurrentScore);
        
        _view.RestartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        _view.GameObjectPanel.gameObject.SetActive(false);
    }
}
