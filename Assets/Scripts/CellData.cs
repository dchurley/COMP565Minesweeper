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
    bool flagged; //is it flagged
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

        flagged = !flagged;

        var tr = gameObject.transform;
        if(flagged )
        {
            tr.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            tr.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void click()
    {
        if(selected || flagged)
        {
            return;
        }
        selected = true;

        var tr = gameObject.transform;
        tr.GetChild(1).gameObject.SetActive(false);
        if (isBomb)
        {
            tr.GetChild(2).gameObject.SetActive(true);
            tr.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            tr.GetChild(0).GetComponent<Renderer>().material = cellMaterials[cellValue];
        }

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
}
