using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject configScreen;

    [Header("Paddles")]
    public Image spritePlayer;
    public Image spriteEnemy;

    [Header("Inputs")]
    public TMP_InputField inputFieldPlayer;
    public TMP_InputField inputFieldEnemy;
    public TMP_InputField inputFieldPointsLimit;

    [Header("Buttons")]
    public Button resetButton;
    public List<Button> playerColors;
    public List<Button> enemyColors;

    [Header("High Score Text")]
    public TMP_Text txtPlayerHighScore;
    public TMP_Text txtEnemyHighScore;
    public GameObject ScoreContainer;

    private bool isNameSaved = false;
    private bool isColorSaved = false;
    void Start()
    {
        inputFieldPointsLimit.onValueChanged.AddListener((limit) => 
        {
            if(int.TryParse(limit, out var intLimit))
                SaveController.Instance.SetPointsLimit(intLimit); 
        });
        inputFieldPlayer.onValueChanged.AddListener((name) => 
        {
            name = name.Trim();
            SaveController.Instance.SetName(true, name); 
            if(!string.IsNullOrEmpty(name)) isNameSaved = true;
            EvaluateResetButtonState();
        });
        inputFieldEnemy.onValueChanged.AddListener((name) =>
        {
            name = name.Trim();
            SaveController.Instance.SetName(false, name);
            if (!string.IsNullOrEmpty(name)) isNameSaved = true;
            EvaluateResetButtonState();
        });
        foreach (var button in playerColors)
        {
            button.onClick.AddListener(() => 
            {
                spritePlayer.color = button.colors.normalColor;
                SaveController.Instance.SetColor(true, spritePlayer.color);
                isColorSaved = true;
                EvaluateResetButtonState();
            });
        }
        foreach (var button in enemyColors)
        {
            button.onClick.AddListener(() => 
            {
                spriteEnemy.color = button.colors.normalColor;
                SaveController.Instance.SetColor(false, spriteEnemy.color);
                isColorSaved = true;
                EvaluateResetButtonState();
            });
        }
        ShowHighScores();
        FillFields();
    }

    private void Update()
    {
        if (configScreen.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            configScreen.SetActive(false);
        }
    }

    private void ShowHighScores()
    {
        var playerScore = SaveController.Instance.GetHighScore(true);
        var enemyScore = SaveController.Instance.GetHighScore(false);
        var anyScore = playerScore > 0 || enemyScore > 0;

        if (anyScore)
        {
            txtPlayerHighScore.text = $"{SaveController.Instance.GetName(true)}: {playerScore}";
            txtEnemyHighScore.text = $"{SaveController.Instance.GetName(false)}: {enemyScore}";
            ScoreContainer.SetActive(true);
        }
    }

    private void FillFields()
    {
        spriteEnemy.color = SaveController.Instance.GetColor(false);
        spritePlayer.color = SaveController.Instance.GetColor(true);
        inputFieldPlayer.text = SaveController.Instance.GetName(true);
        inputFieldEnemy.text = SaveController.Instance.GetName(false);
        inputFieldPointsLimit.text = SaveController.Instance.GetPointsLimit().ToString();

        isColorSaved = spriteEnemy.color != Color.white || spritePlayer.color != Color.white;
        isNameSaved = !string.IsNullOrEmpty(inputFieldPlayer.text) || !string.IsNullOrEmpty(inputFieldEnemy.text);

        EvaluateResetButtonState();
    }

    public void Reset()
    {
        SaveController.Instance.Reset();
        FillFields();
        ScoreContainer.SetActive(false);
        isNameSaved = false;
        isColorSaved = false;
        EvaluateResetButtonState();
    }

    public void EvaluateResetButtonState()
    {
        resetButton.gameObject.SetActive(isNameSaved || isColorSaved);
    }
}
