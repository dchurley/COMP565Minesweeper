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

    public static int MINES;
    public static int WIDTH;
    public static int HEIGHT;

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
        //set up a virtual game to get the logic right
        int[,] virtualGrid = GenerateBoard(WIDTH, HEIGHT, MINES);
        minesText.text = $"Mines: {MINES}";



        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {

                var go = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity);
                go.transform.position = new Vector3(i,0,j);
                go.transform.name = $"[{i},{j}]";

                var cd = go.GetComponent<CellData>();

                go.transform.GetChild(2).gameObject.SetActive(false);

                cd.r = i;
                cd.c = j;
                cd.cellValue = virtualGrid[i,j];
                cd.isBomb = virtualGrid[i,j] == -1 ? true: false;

            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out tmpHitHighlight, 100))
            {
                Debug.Log($"We have a hit: {tmpHitHighlight.transform.name}");
                //get the parent of the hit obj
                var par = tmpHitHighlight.transform.parent;
                var cd = par.gameObject.GetComponent<CellData>();

                cd.click();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out tmpHitHighlight, 100))
            {
                Debug.Log($"We have a hit: {tmpHitHighlight.transform.name}");
                //get the parent of the hit obj
                var par = tmpHitHighlight.transform.parent;
                var cd = par.gameObject.GetComponent<CellData>();

                bool flagged = cd.toggleFlag();

                MINES += flagged ? -1 : 1;

                minesText.text = $"Mines: {MINES}";
            }
        }
    }
}
