using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public int levelIndex;
    // Start is called before the first frame update
    public void setIndex(int index)
    {
        PlayerPrefs.SetInt("Selected", index);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
