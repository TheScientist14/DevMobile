using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Proto
{
    public class LifeText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        [SerializeField]
        private SpaceShipControls _spaceShip;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        private void LateUpdate()
        {
            _text.text = $"X{_spaceShip.Lifes}";
        }
    }
}