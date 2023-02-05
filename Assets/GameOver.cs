using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameOver : MonoBehaviour
{

    public GameState GameState;
    [FormerlySerializedAs("GameOver")] public GameObject GameOverPanel;
    void Update()
    {
        if (GameState.cash <= 0)
        {
            GameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
