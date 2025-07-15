using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    [SerializeField] private int PointsLimit = 5;

    private Color colorPlayer = Color.white;
    private Color colorEnemy = Color.white;
    
    private string namePlayer = "";
    private string nameEnemy = "";

    private int playerHighScore = 0;
    private int enemyHighScore = 0;

    #region Player prefs keys
    private const string POINTS_LIMIT_KEY = "POINTS_LIMIT";

    private const string PLAYER_COLOR_KEY = "PLAYER_COLOR";
    private const string PLAYER_NAME_KEY = "PLAYER_NAME";

    private const string ENEMY_COLOR_KEY = "ENEMY_COLOR";
    private const string ENEMY_NAME_KEY = "ENEMY_NAME";

    private const string PLAYER_HIGHSCORE_KEY = "PLAYER_HIGHSCORE";
    private const string ENEMY_HIGHSCORE_KEY = "ENEMY_HIGHSCORE";
    #endregion

    private static SaveController _instance;
    public static SaveController Instance 
    { 
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<SaveController>();

                if (_instance == null)
                {
                    var singletonObj = new GameObject(typeof(SaveController).Name);
                    _instance = singletonObj.AddComponent<SaveController>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        LoadSavedKeys();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetName(bool isPlayer, string name)
    {
        if (string.IsNullOrEmpty(name)) return;

        if (isPlayer)
        {
            namePlayer = name;
            PlayerPrefs.SetString(PLAYER_NAME_KEY, namePlayer);
        }
        else 
        {
            nameEnemy = name;
            PlayerPrefs.SetString(ENEMY_NAME_KEY, nameEnemy);
        }
        PlayerPrefs.Save();
    }

    public string GetName(bool isPlayer)
    {
        if (isPlayer)
        {
            var name = !string.IsNullOrEmpty(namePlayer) ? namePlayer : "Jogador 1";
            return name;
        }
        else 
        {
            var name = !string.IsNullOrEmpty(nameEnemy) ? nameEnemy : "Jogador 2";
            return name;
        }
    }
    public void SetColor(bool isPlayer, Color color)
    {
        var colorStr = ColorUtility.ToHtmlStringRGBA(color);
        if (isPlayer)
        {
            colorPlayer = color;
            PlayerPrefs.SetString(PLAYER_COLOR_KEY, colorStr);
        }
        else
        {
            colorEnemy = color;
            PlayerPrefs.SetString(ENEMY_COLOR_KEY, colorStr);
        }
        PlayerPrefs.Save();
    }

    public Color GetColor(bool isPlayer)
    {
        return isPlayer ? colorPlayer : colorEnemy;
    }

    public void SetPointsLimit(int limit)
    {
        PointsLimit = limit;
        PlayerPrefs.SetInt(POINTS_LIMIT_KEY, limit);
        PlayerPrefs.Save();
    }

    public int GetPointsLimit()
    {
        return PointsLimit;
    }

    public void SetHighScore(bool isPlayer, int score)
    {
        if (isPlayer)
        {
            playerHighScore = score;
            PlayerPrefs.SetInt(PLAYER_HIGHSCORE_KEY, score);
        }
        else
        {
            enemyHighScore = score;
            PlayerPrefs.SetInt(ENEMY_HIGHSCORE_KEY, score);
        }
        PlayerPrefs.Save();
    }

    public int GetHighScore(bool isPlayer)
    {
        return isPlayer ? playerHighScore : enemyHighScore;
    }

    private void LoadSavedKeys()
    {
        PointsLimit = PlayerPrefs.GetInt(POINTS_LIMIT_KEY, 5);

        namePlayer = PlayerPrefs.GetString(PLAYER_NAME_KEY, "");
        nameEnemy = PlayerPrefs.GetString(ENEMY_NAME_KEY, "");

        if (ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString(PLAYER_COLOR_KEY, "FFFFFFFF"), out Color loadedPlayerColor))
        {
            colorPlayer = loadedPlayerColor;
        }
        if (ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString(ENEMY_COLOR_KEY, "FFFFFFFF"), out Color loadedEnemyColor))
        {
            colorEnemy = loadedEnemyColor;
        }

        playerHighScore = PlayerPrefs.GetInt(PLAYER_HIGHSCORE_KEY, 0);
        enemyHighScore = PlayerPrefs.GetInt(ENEMY_HIGHSCORE_KEY, 0);
    }

    public void Reset()
    {
        colorPlayer = Color.white;
        colorEnemy = Color.white;
        namePlayer = "";
        nameEnemy = "";
        playerHighScore = 0;
        enemyHighScore = 0;
        PointsLimit = 5;
        PlayerPrefs.DeleteAll();
    }
}
