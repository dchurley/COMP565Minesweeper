using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minesweeper : MonoBehaviour
{

    //public Material cellMaterial;

    public RaycastHit tmpHitHighlight;
    public GameObject cellPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                //var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var go = GameObject.Instantiate(cellPrefab, Vector3.zero, Quaternion.identity);
                go.transform.position = new Vector3(i,0,j);
                //go.transform.localScale = new Vector3(1, 0.1f, 1);
                go.transform.name = $"[{i},{j}]";
                //go.transform.GetComponent<Renderer>().material = cellMaterial;

                var cd = go.GetComponent<CellData>();

                go.transform.GetChild(2).gameObject.SetActive(false);

                if(Random.Range(1,10) > 5)
                {
                    cd.isBomb = true;
                    //go.transform.GetComponent<Renderer>().material.color = Color.red;
                    //go.GetComponentInChildren<Renderer>().material.color = Color.red;
                }
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

                par.GetChild(1).gameObject.SetActive(false);
                if(cd.isBomb)
                {
                    par.GetChild(2).gameObject.SetActive(true);
                    par.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
                //if(tmpHitHighlight.transform.name.Equals("cover cube"))
                //{
                //    tmpHitHighlight.transform.gameObject.SetActive(false); 

                //}
            }
        }
    }
}
