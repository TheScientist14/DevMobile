using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Proto
{
    public class ScoreText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        [SerializeField]
        private ScoreData _scoreData;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        private void LateUpdate()
        {
            _text.text = $"{_scoreData.GetScore()}";
        }
    }
}