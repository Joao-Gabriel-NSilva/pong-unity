using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int winPoints = 5;

    [Header("Paddles")]
    public Transform playerPaddle;
    public Transform enemyPaddle;

    [Header("Texts")]
    public TextMeshProUGUI txtPlayerPoints;
    public TextMeshProUGUI txtEnemyPoints;
    public TextMeshProUGUI txtWinner;
    public TextMeshProUGUI txtCountDown;

    [Header("Others")]
    public BallController ballController;
    public GameObject screenEndGame;
    public GameObject screenPause;

    private int playerScore = 0;
    private int enemyScore = 0;
    private int playerHighScore = 0;
    private int enemyHighScore = 0;
    private bool isPaused = false;
    private Coroutine countDownCoroutine;

    void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        CheckPause();
    }

    private void CheckPause()
    {
        if (countDownCoroutine != null) return;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            screenPause.SetActive(isPaused);
            Time.timeScale = isPaused ? 0 : 1;
        }
    }

    public void ResetGame()
    {
        playerPaddle.position = new Vector3(-7, 0, 0);
        enemyPaddle.position = new Vector3(7, 0, 0);

        ballController.ResetBall();

        playerScore = 0;
        enemyScore = 0;
        playerHighScore = SaveController.Instance.GetHighScore(true);
        enemyHighScore = SaveController.Instance.GetHighScore(false);

        txtPlayerPoints.text = playerScore.ToString();
        txtEnemyPoints.text  = enemyScore.ToString();

        txtWinner.text = "";

        screenPause.SetActive(false);
        isPaused = false;

        winPoints = SaveController.Instance.GetPointsLimit();
        countDownCoroutine = StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        txtCountDown.gameObject.SetActive(true);
        Time.timeScale = 0;
        for (int i = 3; i > 0; i--)
        {
            txtCountDown.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        txtCountDown.gameObject.SetActive(false);
        Time.timeScale = 1;
        countDownCoroutine = null;
    }

    public void ScorePlayer()
    {
        playerScore++;
        txtPlayerPoints.text = playerScore.ToString();
        if (ballController.startVelocity.x < 0)
            ballController.startVelocity = new Vector2(Mathf.Abs(ballController.startVelocity.x), ballController.startVelocity.y);
        if (playerScore > playerHighScore)
            SaveController.Instance.SetHighScore(true, playerScore);
        CheckWin();
    }
    
    public void ScoreEnemy()
    {
        enemyScore++;
        txtEnemyPoints.text = enemyScore.ToString();
        if (ballController.startVelocity.x > 0)
            ballController.startVelocity = new Vector2(ballController.startVelocity.x * -1, ballController.startVelocity.y);
        if (enemyScore > enemyHighScore)
            SaveController.Instance.SetHighScore(false, enemyScore);
        CheckWin();
    }

    public void CheckWin()
    {
        var enemyWin = enemyScore >= winPoints;
        var playerWin = playerScore >= winPoints;
        
        if (enemyWin || playerWin)
        {
            var name = SaveController.Instance.GetName(playerWin);
            if(!string.IsNullOrEmpty(name)) 
                txtWinner.text =  "Vitória de " + name;
            else
                txtWinner.text = "Vitória do jogador " + (playerWin ? "1" : "2");
            EndGame();
        }
    }

    public void EndGame()
    {
        ballController.StopBall();
        screenEndGame.SetActive(true);
        Invoke(nameof(LoadMenu), 2.5f);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
