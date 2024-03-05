using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CellData : MonoBehaviour
{

    public Material[] cellMaterials;

    public bool isBomb; //is it a bomb
    public int cellValue; //number of surrounding bombs

    //row and column for searching later
    public int r;
    public int c;

    public bool selected; //is it selected
    public bool flagged; //is it flagged
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void toggleFlag()
    {
        if(selected)
        {
            return;
        }
        //change the flagged state
        flagged = !flagged;

        var tr = gameObject.transform;
        if(flagged )
        {
            //set green if flagged
            tr.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            //set white for unflagged
            tr.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.white;
        }

        return;
    }

    public void click()
    {
        //if it's already clicked or flagged then do nothing
        if(selected || flagged)
        {
            return;
        }

        Minesweeper.hiddenCells--;
        reveal();

        //if it's a 0 cell, then open up all the squares around it automatically because they are guaranteed safe
        if(cellValue == 0)
        {
            int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
            for (int k = 0; k < 8; k++)
            {
                int ni = r + dx[k], nj = c + dy[k];
                var adjacent = GameObject.Find($"[{ni},{nj}]");
                if (adjacent != null)
                {
                    var adjCd = adjacent.GetComponent<CellData>();
                    adjCd.click();
                }
            }
        }
    }

    public void reveal()
    {
        selected = true;

        var tr = gameObject.transform;
        tr.GetChild(1).gameObject.SetActive(false);
        if (isBomb)
        {
            //activate the sphere and turn it red
            tr.GetChild(2).gameObject.SetActive(true);
            tr.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            //set the material to the number texture
            tr.GetChild(0).GetComponent<Renderer>().material = cellMaterials[cellValue];
        }
    }
}
