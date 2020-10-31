/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapGenerator : MonoBehaviour
{
    //These increments are used for the unity Transform positions
    private float xIncrement = 0.86f;
    private float zIncrement = 1.5f;

    private int x;
    private int y;
    private int z;

    private Vector3 hexLocation;

    //These values here are for the 3 dimensional hex positions
    //Ask Moni for further clarifications
    public int positiveX;
    public int negativeX;

    public int positiveY;
    public int negativeY;

    public int positiveZ;
    public int negativeZ;

    public GameObject hexPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //This algorithm is for me to understand a bit, I'll entirely change this
        int counter;

        for (counter = -negativeX; counter <= positiveX; counter++) //This places the hexagons on the x Axis
        {
            hexLocation.Set(xIncrement * counter * 2, 0f, 0f);
            Instantiate(hexPrefab, hexLocation, Quaternion.Euler(-90,0,0));
        }

        for (counter = -negativeY; counter <= positiveY; counter++) //This places the hexagons on the y Axis
        {
            hexLocation.Set(xIncrement * counter, 0f, zIncrement * counter);
            Instantiate(hexPrefab, hexLocation, Quaternion.Euler(-90, 0, 0));
        }

        for (counter = -negativeZ; counter <= positiveZ; counter++) //This places the hexagons on the z Axis
        {
            hexLocation.Set(-xIncrement * counter, 0f, zIncrement * counter);
            Instantiate(hexPrefab, hexLocation, Quaternion.Euler(-90, 0, 0));
        }



        /*for (z = negativeZ; z < positiveZ; ++z)
        {
            for (y = negativeY; y < positiveY; ++y)
            {
                for (x = negativeX; x < positiveX; ++x)
                {
                    hexLocation.Set();
                }
            }
        }*/
    }
}
*/


using UnityEngine;
using System.Collections;

public class HexMapGenerator : MonoBehaviour
{
    public GameObject hexPrefab;

    //Size of the map in terms of number of hex tiles
    //Not representative of the amount of world space
    int width = 15;
    int height = 18;

    float xOffset = 1.8f;
    float yOffset = 1.55f;

    void Start()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xPos = x * xOffset;

                if (y % 2 == 1)
                {
                    xPos += (xOffset / 2);
                }

                //make the hexy boi
                GameObject hex_go = (GameObject)Instantiate(hexPrefab, new Vector3(xPos,0,y * yOffset), Quaternion.identity);

                //Name and assign array values
                hex_go.name = "Hex_ " + x + "_" + y;

                //Clean up heirarchy by putting it with "map"
                hex_go.transform.SetParent(this.transform);
                hex_go.isStatic = true;
            }
        }
    }
}