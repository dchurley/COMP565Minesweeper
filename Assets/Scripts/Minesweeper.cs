using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Minesweeper : MonoBehaviour
{

    public RaycastHit tmpHitHighlight;
    public GameObject cellPrefab;
    public TextMeshProUGUI minesText;
    public TextMeshProUGUI resultText;
    public GameObject button;
    public Transform cameraTransform;

    public static int MINES;
    public static int WIDTH;
    public static int HEIGHT;

    public static int hiddenCells;
    public static int remainingMines;

    bool gameOver = false;

    static System.Random random = new System.Random();

    public static int[,] GenerateBoard(int width, int height, int totalMines)
    {
        int[,] board = new int[height, width];

        // Place mines randomly
        for (int i = 0; i < totalMines; i++)
        {
            int x, y;
            do
            {
                x = random.Next(width);
                y = random.Next(height);
            } while (board[y, x] == -1); // Ensure we don't place a mine on top of another mine

            board[y, x] = -1; // -1 represents a mine
        }

        // Fill in numbers indicating the number of adjacent mines for each cell
        // Iterate over each cell
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // If the cell is not a mine, calculate the number of adjacent mines
                if (board[y, x] != -1)
                {
                    int count = CountAdjacentMines(board, x, y);
                    board[y, x] = count;
                }
            }
        }

        return board;
    }

    static int CountAdjacentMines(int[,] board, int x, int y)
    {
        int height = board.GetLength(0);
        int width = board.GetLength(1);
        int count = 0;

        // Iterate over adjacent cells
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int neighborX = x + i;
                int neighborY = y + j;

                // Check if neighbor cell is within bounds and contains a mine
                if (neighborX >= 0 && neighborX < width &&
                    neighborY >= 0 && neighborY < height &&
                    board[neighborY, neighborX] == -1)
                {
                    count++;
                }
            }
        }

        return count;
    }


    void Start()
    {

        //move the camera
        //if it's medium or hard move it, if its easy then the camera is fine
        if (MINES == 20)
        {
            cameraTransform.position = new Vector3(4.0f, 11.14f, 8.52f);
        }
        if (MINES == 40)
        {
            cameraTransform.position = new Vector3(5.7f, 12.98f, 9.81f);
        }

        hiddenCells = WIDTH * HEIGHT;

        //hide the text and button
        resultText.gameObject.SetActive(false);
        button.SetActive(false);


        //set up a virtual game to get the logic right
        int[,] virtualGrid = GenerateBoard(WIDTH, HEIGHT, MINES);
        minesText.text = $"Mines: {MINES}";
        remainingMines = MINES;


        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {

                var go = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity);
                go.transform.position = new Vector3(i, 0, j);
                go.transform.name = $"[{i},{j}]";

                var cd = go.GetComponent<CellData>();

                go.transform.GetChild(2).gameObject.SetActive(false);

                cd.r = i;
                cd.c = j;
                cd.cellValue = virtualGrid[i, j];
                cd.isBomb = virtualGrid[i, j] == -1 ? true : false;

            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //for left click
        if (Input.GetMouseButtonDown(0) && !gameOver)
        {
            if (Physics.Raycast(ray, out tmpHitHighlight, 100))
            {
                Debug.Log($"We have a hit: {tmpHitHighlight.transform.name}");
                //get the parent of the hit obj
                var par = tmpHitHighlight.transform.parent;
                var cd = par.gameObject.GetComponent<CellData>();

                cd.click();
                if (cd.isBomb)
                {
                    gameOver = true;
                    resultText.text = "YOU LOST!";
                    resultText.gameObject.SetActive(true);
                    button.SetActive(true);

                    for (int i = 0; i < HEIGHT; i++)
                    {
                        for (int j = 0; j < WIDTH; j++)
                        {

                            var cell = GameObject.Find($"[{i},{j}]");
                            if (cell != null)
                            {
                                var cellData = cell.GetComponent<CellData>();
                                cellData.reveal();
                            }

                        }
                    }
                }
                else if (hiddenCells == MINES)
                {
                    gameOver = true;
                    resultText.text = "YOU WON!";
                    resultText.gameObject.SetActive(true);
                    button.SetActive(true);
                }




            }
        }

        //for right click
        if (Input.GetMouseButtonDown(1) && !gameOver)
        {
            if (Physics.Raycast(ray, out tmpHitHighlight, 100))
            {
                Debug.Log($"We have a hit: {tmpHitHighlight.transform.name}");
                //get the parent of the hit obj
                var par = tmpHitHighlight.transform.parent;
                var cd = par.gameObject.GetComponent<CellData>();

                if(!cd.flagged && remainingMines == 0)
                {
                    return;
                }
                cd.toggleFlag();
                if (!cd.selected)
                {
                    remainingMines += cd.flagged ? -1 : 1;
                }

                minesText.text = $"Mines: {remainingMines}";
            }
        }
    }
}
