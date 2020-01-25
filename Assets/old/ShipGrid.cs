using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipGrid : MonoBehaviour
{
    public GameObject slot;

    public int gridSizeX;
    public int gridSizeY;
    public GameObject[,] slots;

    public void Start()
    {
        slots = new GameObject[gridSizeX, gridSizeY];

        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();


        int count = 0;
        for (int i = 0; i < this.gridSizeX; i++)
        {
            for (int j = 0; j < this.gridSizeY; j++)
            {
                count++;
                GameObject newSlot = Instantiate(slot, Vector3.zero, Quaternion.identity);
                newSlot.name = "slot " + count;
                newSlot.transform.SetParent(grid.transform, true);
                slots[i, j] = newSlot;
            }
        }

        // Resize the GridLayoutGroup to fit the children.
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = this.gridSizeX;
        LayoutRebuilder.ForceRebuildLayoutImmediate(grid.GetComponent<RectTransform>());

        //for (int i = 0; i < this.gridSizeX; i++)
        //{
        //    for (int j = 0; j < this.gridSizeY; j++)
        //    {
        //        GameObject gridSlot = slots[i, j];
        //        RectTransform tx = gridSlot.GetComponent<RectTransform>();
        //        //tx.anchoredPosition = grid.GetComponent<RectTransform>().anchoredPosition;
        //    }
        //}
        grid.GetComponent<ContentSizeFitter>().enabled = false;
    }
}
