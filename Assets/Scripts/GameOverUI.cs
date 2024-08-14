using System;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        PlayerHealth.GameOverEvent += GameOver;
    }

    private void OnDisable()
    {
        PlayerHealth.GameOverEvent -= GameOver;
    }

    private void GameOver()
    {
        canvas.enabled = true;
    }
}
