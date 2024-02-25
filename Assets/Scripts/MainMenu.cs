using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void playEasy()
    {
        Minesweeper.MINES = 20;
        Minesweeper.WIDTH = 9;
        Minesweeper.HEIGHT = 9;
        SceneManager.LoadScene("Minesweeper");
    }

    public void playMedium()
    {
        Minesweeper.MINES = 40;
        Minesweeper.WIDTH = 12;
        Minesweeper.HEIGHT = 10;
        SceneManager.LoadScene("Minesweeper");
    }

    public void playHard()
    {
        Minesweeper.MINES = 80;
        Minesweeper.WIDTH = 14;
        Minesweeper.HEIGHT = 12;
        SceneManager.LoadScene("Minesweeper");
    }

    public void playGame()
    {
        Minesweeper.MINES = 10;
        Minesweeper.WIDTH = 10;
        Minesweeper.HEIGHT = 10;
        SceneManager.LoadScene("Minesweeper");
    }
}
