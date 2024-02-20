using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartSO : MonoBehaviour
{
    [SerializeField]
    AsteroidInfo _asteroidInfo;
    [SerializeField]
    ScoreData _scoreData;

    private void ResetSO()
    {
        _scoreData.ResetScore();
        _asteroidInfo.ResetValues();
    }

    public void RestartGame()
    {
        ResetSO();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    void Awake()
    {
        ResetSO();
    }

}
