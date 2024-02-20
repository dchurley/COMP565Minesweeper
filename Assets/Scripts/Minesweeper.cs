using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Minesweeper : MonoBehaviour
{

    //public Material cellMaterial;

    public RaycastHit tmpHitHighlight;
    public GameObject cellPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //set up a virtual game to get the logic right
        int ROWS = 9;
        int COLUMNS = 9;
        int[][] virtualGrid = new int[ROWS][];

        for(int i = 0; i < ROWS; i++) {
            virtualGrid[i] = new int[COLUMNS];
        }

        //create some bombs
        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                if (Random.Range(1, 10) > 8)
                {
                    //-1 will mean bomb
                    virtualGrid[i][j] = -1;
                }
            }
        }

        //for the rest of the cells, generate their value of bombs nearby
        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                //skip if this cell is a bomb, it doesnt need a bomb count
                if (virtualGrid[i][j] == -1)
                {
                    continue;
                }
                int bombCount = 0;
                //for the 8 cells around this cell...
                int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
                int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
                for (int k = 0; k < 8; k++)
                {
                    int ni = i + dx[k], nj = j + dy[k];
                    if (ni >= 0 && ni < ROWS && nj >= 0 && nj < COLUMNS && virtualGrid[ni][nj] == -1)
                    {
                        bombCount++;
                    }
                }

                virtualGrid[i][j] = bombCount;
            }
        }


        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLUMNS; j++)
            {

                var go = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity);
                go.transform.position = new Vector3(i,0,j);
                go.transform.name = $"[{i},{j}]";

                var cd = go.GetComponent<CellData>();

                go.transform.GetChild(2).gameObject.SetActive(false);

                cd.r = i;
                cd.c = j;
                cd.cellValue = virtualGrid[i][j];
                cd.isBomb = virtualGrid[i][j] == -1 ? true: false;

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

                cd.toggleFlag();
            }
        }
    }
}
