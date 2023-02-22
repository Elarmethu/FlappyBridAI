using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private GameObject _playerPrefab;
    public GameObject playerPrefab => _playerPrefab;

    [Header("UI References")]
    [SerializeField] private Text _inforamtionText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _gameOver;

    private int _score;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    public void ResetGame()
    {
        _score = 0;
        _scoreText.text = _score.ToString();

        Pipes[] pipes = FindObjectsOfType<Pipes>();
        Player[] birds = FindObjectsOfType<Player>();

        Spawner.instance.ClearPipeList();

        for (int i = 0; i < pipes.Length; i++)
            Destroy(pipes[i].gameObject);

        for (int i = 0; i < birds.Length; i++)
            Destroy(birds[i].gameObject);
    }

    public void IncreaseScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }

    public void InformationNeural(int gen, float best, float fit)
    {
        _inforamtionText.text = $"GEN: {gen}\n";
        _inforamtionText.text += $"BEST FIT: {Math.Round(best, 2)}\n";
        _inforamtionText.text += $"FIT: {Math.Round(fit, 2)}";
    }
}
