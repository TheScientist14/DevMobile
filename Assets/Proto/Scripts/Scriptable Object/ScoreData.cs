using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto
{
    [CreateAssetMenu(menuName = "Data/Score")]
    public class ScoreData : ScriptableObject
    {
        [ReadOnly]
        private int _Score;
        private int _ModuloTracker = 0;

        [SerializeField]
        private int _NewLifeScore = 10000;
        public SpaceShipControls _spaceShip;

        public void AddScore(int score)
        {
            _Score += score;
            int mod = _Score % _NewLifeScore;
            if (_ModuloTracker > mod && _spaceShip != null)
                _spaceShip.Lifes++;
            _ModuloTracker = mod;
        }
        public void ResetScore()
        {
            _ModuloTracker = 0;
            _Score = 0;
        }
        public int GetScore()
        {
            return _Score;
        }
    }
}