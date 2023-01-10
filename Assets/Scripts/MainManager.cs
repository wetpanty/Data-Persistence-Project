using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public static int bestScore = 0;
    public static string bestPlayer = "";
    private string userName = "";
    [SerializeField] Text nameText;
    [SerializeField] Text bestScoreText;

    
    // Start is called before the first frame update
    void Start()
    {
        LoadRecords();
        bestScoreText.text = $"Best Score : {bestScore} Name : {bestPlayer}";

        if (InputName.Instance != null)
        {
            userName = InputName.userName;
            nameText.text = "Name : " + userName;
        }
        else
        {
            Debug.Log("No instance");
        }
        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if(m_Points >= bestScore)
        {
            bestScore = m_Points;
            bestPlayer = userName;
            bestScoreText.text = $"Best Score : {bestScore} Name : {bestPlayer}";//Remember to add .text
            SaveRecords();
        }
    }

    [System.Serializable]
   class SaveData
    {
        public int bestScore;
        public string bestPlayer;
    }

    public static void SaveRecords()//static methods can only use static variables
    {
        var SaveDataInstance = new SaveData();

        SaveDataInstance.bestScore = bestScore;
        SaveDataInstance.bestPlayer = bestPlayer;

        string jsonData = JsonUtility.ToJson(SaveDataInstance);

        File.WriteAllText(Application.persistentDataPath + "/DDPsavefile.json", jsonData);
        Debug.Log("Records saved");

    }

    public static void LoadRecords()
    {
        string path = Application.persistentDataPath + "/DDPsavefile.json";
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            var SaveDataInstance = JsonUtility.FromJson<SaveData>(jsonData);
            bestPlayer = SaveDataInstance.bestPlayer;
            bestScore = SaveDataInstance.bestScore;
            Debug.Log("Records loaded");
        }
    }
}
