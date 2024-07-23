using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static bool gameOver;
    public float timer = 60;
    public Text timeText;
    private bool hasStarted = false;
    public GameObject msg;

    [Space(20)]
    [Header("Worlds")]
    public int currentLevel;
    public World world1, world2, world3;

    [Space(20)]
    [Header("Level Complete")]
    public GameObject canvas;
    public Text levelText;
    public Text gameTime;
    public GameObject nextLevelButton, restartLevelButton;

    private void Awake()
    {
        Time.timeScale = 1;
        gameOver = false;
        LevelInitialize();
    }

    void LevelInitialize()
    {
        currentLevel = PlayerPrefs.GetInt("Selected", 0);
        if(currentLevel < 20)
        {
            // world 1
            world1.init(currentLevel);
        }
        else if(currentLevel < 40)
        {
            // world 2
            world2.init(currentLevel - 20);
        }
        else
        {
            // world 3
            world3.init(currentLevel - 40);
        }

        DestroyUnActiveLevels();
    }
    private void FixedUpdate()
    {
        if(hasStarted)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                if(!gameOver)
                {
                    LevelComplete(false);
                }
                
            }
            else
                timeText.text = timer.ToString("0.00");

        }
    }

    public void BoolTimer(bool flag)
    {
        hasStarted = flag;
    }

    public static void PRINT(string t)
    {
        if (gameOver) return;
        GameManager gm = FindObjectOfType<GameManager>();
        GameObject obj = Instantiate(gm.msg);
        obj.GetComponentInChildren<Text>().text = t;
        Destroy(obj, 3);
    }

    public void LevelComplete(bool success)
    {
        BoolTimer(false);
        gameOver = true;
        canvas.SetActive(true);

        if(success)
        {
            levelText.text = "Complete";
            levelText.color = Color.green;
            nextLevelButton.SetActive(true);
            LevelUnlock();
        }
        else
        {
            levelText.text = "Fail";
            levelText.color = Color.red;
            restartLevelButton.SetActive(true);
        }
        gameTime.text = TimeFormat(timer);
    }

    public void Pause(bool pause)
    {
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    string TimeFormat(float value)
    {
        float minutes = value / 60;
        float seconds = value % 60;
        float milliseconds = ((value - Mathf.Floor(value)) * 1000);

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public static Task delay(float seconds)
    {
        return Task.Delay((int)(seconds * 1000));
    }

    public void LoadNextLevel()
    {
        PlayerPrefs.SetInt("Selected", currentLevel + 1);
        changescene(1);
    }

    void LevelUnlock()
    {
        int highestLevelUnlocked = PlayerPrefs.GetInt("unlock", 1);
        if(currentLevel > highestLevelUnlocked)
        {
            PlayerPrefs.SetInt("unlock", currentLevel);
        }
    }

    public void changescene(int lvl)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(lvl);
    }

    void DestroyUnActiveLevels()
    {
        world1.DestroyObj();
        world2.DestroyObj();
        world3.DestroyObj();
    }

    [System.Serializable]
    public class World
    {
        public Material skybox;
        public GameObject[] levels;

        public void init(int SavedLevel)
        {
            RenderSettings.skybox = skybox;
            levels[SavedLevel].SetActive(true);
        }

        public void DestroyObj()
        {
            for (int i = 0; i < levels.Length; i++)
            {
                if (!levels[i].activeInHierarchy)
                    Destroy(levels[i]);
            }
        }
    }
}
