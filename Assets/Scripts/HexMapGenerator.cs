using UnityEngine;
using System.Collections;

public class HexMapGenerator : MonoBehaviour
{
    public GameObject hexPrefab;

    //Size of the map in terms of number of hex tiles
    //Not representative of the amount of world space
    private int width = 9;
    private int height = 10;

    private float xOffset = 1.8f;
    private float yOffset = 1.55f;

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
                hex_go.name = "Hex_" + x + "_" + y;

                //Position awareness
                hex_go.GetComponent<Hex>().setX(x);
                hex_go.GetComponent<Hex>().sety(y);

                //Clean up heirarchy by putting it with "map"
                hex_go.transform.SetParent(this.transform);
                hex_go.isStatic = true;
            }
        }
    }
}
