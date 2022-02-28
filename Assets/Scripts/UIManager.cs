using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    //Settings


    // Connections
    public GameManager gameManager;

    // State Variables

    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        //InitState();
    }
    void InitConnections()
    {
    }
    void InitState()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetRowCount(float rowCount)
    {
        gameManager.rowCount = Mathf.RoundToInt(rowCount);
    }

    public void SetColumnCount(float columnCount)
    {
        gameManager.columnCount = Mathf.RoundToInt(columnCount);
    }

    public void SetColorCount(float colorCount)
    {
        gameManager.colorCount = Mathf.RoundToInt(colorCount);
    }

    public void SetFirstCondition(string input)
    {
        int count;
        if (int.TryParse(input, out count))
        {
            gameManager.tilesRequiredPerStage[0] = count;
        }
    }

    public void SetSecondCondition(string input)
    {
        int count;
        if (int.TryParse(input, out count))
        {
            gameManager.tilesRequiredPerStage[1] = count;
        }
    }

    public void SetThirdCondition(string input)
    {
        int count;
        if (int.TryParse(input, out count))
        {
            gameManager.tilesRequiredPerStage[2] = count;
        }
    }
}