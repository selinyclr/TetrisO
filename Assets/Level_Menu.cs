using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Menu : MonoBehaviour
{
    public void level1()
    {
        SceneManager.LoadScene("Tetris/Scenes3/Tetris1Scene");
    }

    public void level2()
    {
        SceneManager.LoadScene("Scenes/Tetris");
    }

    public void level3()
    {
        SceneManager.LoadScene("Tetris3/Scenes4/Tetris3Scene");
    }

    public void level4()
    {
        SceneManager.LoadScene("Tetris4/Scenes/Tetris4Scene");
    }
}
