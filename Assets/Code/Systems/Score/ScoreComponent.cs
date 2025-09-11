using System;
using UnityEngine;

namespace Systems.Score
{
    public class ScoreComponent : MonoBehaviour
    {
        private int _currentScore;

        public int CurrentScore => _currentScore;

        public event Action ScoreChanged;

        public void AddScore(int score)
        {
            _currentScore += score;
            ScoreChanged?.Invoke();
        }

        public void ResetScore()
        {
            _currentScore = 0;
            ScoreChanged?.Invoke();
        }
    }

}