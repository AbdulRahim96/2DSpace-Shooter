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
        levelIndex = index;
    }

    public void Play()
    {
        if(levelIndex != 0)
            SceneManager.LoadScene(levelIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
