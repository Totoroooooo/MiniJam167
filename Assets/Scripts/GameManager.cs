using MiniJam167.Enemy;
using MiniJam167.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MiniJam167
{
    public class GameManager : MonoBehaviour
    {
        public GameObject GameMenuUI;
        public GameObject GameOverUI;
        public GameObject GameWinUI;

        [SerializeField] private EnemyBody _enemyBody;
        [SerializeField] private PlayerBody _playerBody;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _restartButton;

        private void Start()
        {
            _enemyBody.Died += Win;
            _playerBody.Died += GameOver;
            _startButton.onClick.AddListener(GameStart);
            _restartButton.onClick.AddListener(Restart);
        }

        private void OnDestroy()
        {
            _enemyBody.Died -= Win;
            _playerBody.Died -= GameOver;
        }

        public void GameStart()
        {
            GameMenuUI.SetActive(false);
            _enemyBody.Init();
            _playerBody.Init();
        }
        public void GameOver()
        {
            GameOverUI.SetActive(true);
        }
        public void Win()
        {
            GameWinUI.SetActive(true);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
