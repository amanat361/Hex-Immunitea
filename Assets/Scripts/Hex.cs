using System.Collections;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public int x, y, ogX, ogY;
    private GameObject[] neighbors;
    private GameObject mouseMgr;
    private MouseManager mouseManager;
    public GameObject resident = null;

    private void Start()
    {
        mouseMgr = GameObject.Find("MouseMgr");
        mouseManager = mouseMgr.GetComponent<MouseManager>();
        neighbors = new GameObject[22];
    }

    public Hex setX(int a)
    {
        x = a;
        ogX = x;
        return null;
    }
    public Hex sety(int a)
    {
        y = a;
        ogY = y;
        return null;
    }

    public GameObject getResident()
    {
        return resident;
    }

    public void setResident(GameObject r)
    {
        resident = r;
    }

    //GET LEFT NEIGHBOR
    public GameObject GetLeftNeighbor()
    {
        if (x == 0) return this.gameObject;
        GameObject leftNeighbor = GameObject.Find("Hex_" + (x - 1) + "_" + y);
        if (leftNeighbor.GetComponent<Hex>().getResident() != null)
        {
          if (mouseManager.selectedUnit.team == leftNeighbor.GetComponent<Hex>().getResident().GetComponentInParent<Unit>().team) return this.gameObject;
        }
        return leftNeighbor;
    }

    //GET RIGHT NEIGHBOR
    public GameObject GetRightNeighbor()
    {
        if (x == 8) return this.gameObject;
        GameObject rightNeighbor = GameObject.Find("Hex_" + (x + 1) + "_" + y);
        if (rightNeighbor.GetComponent<Hex>().getResident() != null)
        {
          if (mouseManager.selectedUnit.team == rightNeighbor.GetComponent<Hex>().getResident().GetComponentInParent<Unit>().team) return this.gameObject;
        }
        return rightNeighbor;
    }

    //GET UPPER LEFT NEIGHBOR
    public GameObject GetUpperLeftNeighbor()
    {
        GameObject upperLeftNeighbor = this.gameObject;
        if (y == 9) return upperLeftNeighbor;
        if (y % 2 == 1) upperLeftNeighbor = GameObject.Find("Hex_" + x + "_" + (y + 1));
        if (y % 2 == 0 && x != 0) upperLeftNeighbor = GameObject.Find("Hex_" + (x - 1) + "_" + (y + 1));
        return upperLeftNeighbor;
    }

    //GET UPPER RIGHT NEIGHBOR
    public GameObject GetUpperRightNeighbor()
    {
        GameObject upperRightNeighbor = this.gameObject;
        if (y == 9) return upperRightNeighbor;
        if (y % 2 == 0) upperRightNeighbor = GameObject.Find("Hex_" + x + "_" + (y + 1));
        if (y % 2 == 1 && x != 8) upperRightNeighbor = GameObject.Find("Hex_" + (x + 1) + "_" + (y + 1));
        return upperRightNeighbor;
    }

    //GET LOWER LEFT NEIGHBOR
    public GameObject GetLowerLeftNeighbor()
    {
        GameObject lowerLeftNeighbor = this.gameObject;
        if (y == 0) return lowerLeftNeighbor;
        if (y % 2 == 1) lowerLeftNeighbor = GameObject.Find("Hex_" + x + "_" + (y - 1));
        if (y % 2 == 0 && x != 0) lowerLeftNeighbor = GameObject.Find("Hex_" + (x - 1) + "_" + (y - 1));
        return lowerLeftNeighbor;
    }

    //GET LOWER RIGHT NEIGHBOR
    public GameObject GetLowerRightNeighbor()
    {
        GameObject lowerRightNeighbor = this.gameObject;
        if (y == 0) return lowerRightNeighbor;
        if (y % 2 == 0) lowerRightNeighbor = GameObject.Find("Hex_" + x + "_" + (y - 1));
        if (y % 2 == 1 && x != 8) lowerRightNeighbor = GameObject.Find("Hex_" + (x + 1) + "_" + (y - 1));
        return lowerRightNeighbor;
    }

    public GameObject[] getKingSpaces()
    {
        clearNeighbors();
        neighbors[0] = GetLeftNeighbor();
        neighbors[1] = GetRightNeighbor();
        neighbors[2] = GetUpperLeftNeighbor();
        neighbors[3] = GetUpperRightNeighbor();
        neighbors[4] = GetLowerLeftNeighbor();
        neighbors[5] = GetLowerRightNeighbor();
        return neighbors;
    }

    public GameObject[] getPawnSpaces(char team)
    {
        clearNeighbors();
        neighbors[0] = GetLeftNeighbor();
        neighbors[1] = GetRightNeighbor();
        if (team == 'A')
        {
          neighbors[2] = GetUpperLeftNeighbor();
          neighbors[3] = GetUpperRightNeighbor();
        }
        else
        {
          neighbors[2] = GetLowerLeftNeighbor();
          neighbors[3] = GetLowerRightNeighbor();
        }
        return neighbors;
    }

    public GameObject[] getBishopSpaces()
    {
        clearNeighbors();
        int i = 0;

        while (GetUpperLeftNeighbor() != this.gameObject)
        {
          if (GetUpperLeftNeighbor().GetComponent<Hex>().getResident() != null)
          {
            if (GetUpperLeftNeighbor().GetComponent<Hex>().getResident().GetComponent<Unit>().team == mouseManager.selectedUnit.team) break;
          }
          neighbors[i] = GetUpperLeftNeighbor();
          if (GetUpperLeftNeighbor().GetComponent<Hex>().getResident() != null)
          {
            i++;
            break;
          }
          Hex temp = GetUpperLeftNeighbor().GetComponent<Hex>();
          x = temp.x;
          y = temp.y;
          i++;
        }

        x = ogX;
        y = ogY;

        while (GetUpperRightNeighbor() != this.gameObject)
        {
          if (GetUpperRightNeighbor().GetComponent<Hex>().getResident() != null)
          {
            if (GetUpperRightNeighbor().GetComponent<Hex>().getResident().GetComponent<Unit>().team == mouseManager.selectedUnit.team) break;
          }
          neighbors[i] = GetUpperRightNeighbor();
          if (GetUpperRightNeighbor().GetComponent<Hex>().getResident() != null)
          {
            i++;
            break;
          }
          Hex temp = GetUpperRightNeighbor().GetComponent<Hex>();
          x = temp.x;
          y = temp.y;
          i++;
        }

        x = ogX;
        y = ogY;

        while (GetLowerLeftNeighbor() != this.gameObject)
        {
          if (GetLowerLeftNeighbor().GetComponent<Hex>().getResident() != null)
          {
            if (GetLowerLeftNeighbor().GetComponent<Hex>().getResident().GetComponent<Unit>().team == mouseManager.selectedUnit.team) break;
          }
          neighbors[i] = GetLowerLeftNeighbor();
          if (GetLowerLeftNeighbor().GetComponent<Hex>().getResident() != null)
          {
            i++;
            break;
          }
          Hex temp = GetLowerLeftNeighbor().GetComponent<Hex>();
          x = temp.x;
          y = temp.y;
          i++;
        }

        x = ogX;
        y = ogY;

        while (GetLowerRightNeighbor() != this.gameObject)
        {
          if (GetLowerRightNeighbor().GetComponent<Hex>().getResident() != null)
          {
            if (GetLowerRightNeighbor().GetComponent<Hex>().getResident().GetComponent<Unit>().team == mouseManager.selectedUnit.team) break;
          }
          neighbors[i] = GetLowerRightNeighbor();
          if (GetLowerRightNeighbor().GetComponent<Hex>().getResident() != null)
          {
            i++;
            break;
          }
          Hex temp = GetLowerRightNeighbor().GetComponent<Hex>();
          x = temp.x;
          y = temp.y;
          i++;
        }

        x = ogX;
        y = ogY;

        return neighbors;
    }

    public GameObject[] getRookSpaces()
    {
        clearNeighbors();
        int i = 0;
        while (GetLeftNeighbor() != this.gameObject)
        {
            neighbors[i] = GetLeftNeighbor();
            x = GetLeftNeighbor().GetComponent<Hex>().x;
            y = GetLeftNeighbor().GetComponent<Hex>().y;
            i++;
        }
        x = ogX;
        y = ogY;
        while (GetRightNeighbor() != this.gameObject)
        {
            neighbors[i] = GetRightNeighbor();
            x = GetRightNeighbor().GetComponent<Hex>().x;
            y = GetRightNeighbor().GetComponent<Hex>().y;
            i++;
        }
        x = ogX;
        y = ogY;
        neighbors[i] = GetUpperLeftNeighbor();
        i++;
        neighbors[i] = GetUpperRightNeighbor();
        i++;
        neighbors[i] = GetLowerLeftNeighbor();
        i++;
        neighbors[i] = GetLowerRightNeighbor();
        return neighbors;
    }

    public void clearNeighbors()
    {
        for (int i = 0; i < neighbors.Length; i++)
        {
            neighbors[i] = null;
        }
    }
}
