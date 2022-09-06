using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int bestScore;
    public string playerName;
    public string bestScorePlayerName;
    private TMP_Text currentPlayer;
    private TMP_Text bestScoreText;
    private GameObject registerPlayerNameObject;
    private TMP_InputField registerPlayerName;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadScore();
    }

    private void Start()
    {
        registerPlayerNameObject = GameObject.Find("InputField (TMP)");
        registerPlayerName = registerPlayerNameObject.GetComponent<TMP_InputField>();
        currentPlayer = GameObject.Find("CurrentPlayer").GetComponent<TMP_Text>();
        bestScoreText = GameObject.Find("BestScore").GetComponent<TMP_Text>();

        if (bestScore == 0)
        {
            bestScoreText.text = "---------------";
        }
        else
        {
            bestScoreText.text = $"{GameManager.Instance.bestScorePlayerName} : {GameManager.Instance.bestScore}";
        }
    }


    public void ReadStringInput(string s)
    {
        playerName = registerPlayerName.text;
        currentPlayer.text = $"Current player: " + playerName;
        Debug.Log(playerName);
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public int bestScore;
        public string bestScorePlayerName;
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.bestScorePlayerName = bestScorePlayerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.bestScore;
            bestScorePlayerName = data.bestScorePlayerName;
        }
    }
}
