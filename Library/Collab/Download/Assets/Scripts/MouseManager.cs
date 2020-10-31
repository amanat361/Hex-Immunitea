using System.Collections;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private Unit selectedUnit;

    void Update()
    {
        //MAKE A RAYCAST FROM THE MOUSE
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        //EVERY FRAME CHECK IF MOUSE RAY HITS 3D OBJECT
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject ourHitObject = hitInfo.collider.transform.gameObject;

            //IF OBJECT HAS UNIT SCRIPT, RUN THE UNIT FUNCTION
            if (ourHitObject.GetComponent<Unit>() != null)
            {
                MouseOver_Unit(ourHitObject);
            }
            //IF OBJECT HAS HEX SCRIPT, RUN THE HEX FUNCTION
            else if (ourHitObject.GetComponent<HexBody>() != null)
            {
                MouseOver_Hex(ourHitObject);
            }
        }
    }

    void MouseOver_Hex(GameObject ourHitObject)
    {
        //USER LEFT CLICK ON HEX
        if (Input.GetMouseButtonDown(0))
        {
            //IF UNIT IS SELECTED
            if (selectedUnit != null)
            {
                //UNIT BY DEFAULT CANT MOVE TO THAT HEX
                bool canMoveToTile = false;

                //MAKE AN ARRAY OF THE NEIGHBORING HEXES OF UNIT (PLACES IT CAN MOVE)
                GameObject[] tempN = selectedUnit.GetComponent<Unit>().getHouse().GetComponentInChildren<Hex>().getNeighbors();

                //IF THE SELECTED TILE IS AN AVAILABLE HEX AND NOT OCCUPIED BY ANOTHER UNIT, CAN MOVE = TRUE
                for (int i = 0; i < 6; i++)
                {
                    if (tempN[i] != null)
                    {
                        if (tempN[i].gameObject == ourHitObject.transform.parent.gameObject)
                        {
                            canMoveToTile = true;
                            break;
                        }
                    }
                }

                //IF TILE IS OCCUPIED BY ANOTHER UNIT, REMOVE FROM THE ARRAY / MAKE IT NULL
                for (int i = 0; i < 6; i++)
                {
                    if (tempN[i].GetComponent<Hex>().getResident() != null)
                    {
                        tempN[i] = null;
                    }
                }

                //IF CAN MOVE:
                //UNCOLOR AVAILABLE TILES
                //REMOVE LINK BETWEEN UNIT AND CURRENT HEX
                //MOVE UNIT TO NEW HEX
                //ADD LINK BETWEEN UNIT AND THE NEW HEX
                //DESELECT THE UNIT
                if (canMoveToTile)
                {
                  GameObject temp = ourHitObject.GetComponentInParent<Hex>().getResident();
                  if (temp == null || temp.gameObject.tag != selectedUnit.gameObject.tag)
                  {
                    colorNeighbors(Color.white, selectedUnit.getHouse().gameObject, tempN);
                    selectedUnit.getHouse().GetComponent<Hex>().setResident(null);
                    if (temp != null && temp.gameObject.tag != selectedUnit.gameObject.tag) Destroy(temp);
                    selectedUnit.moveTo(ourHitObject);
                    ourHitObject.GetComponentInParent<Hex>().setResident(selectedUnit.transform.gameObject);
                    selectedUnit = null;
                  }
                }
            }
        }
    }

    void MouseOver_Unit(GameObject ourHitObject)
    {
        //SELECT UNIT ON LEFT MOUSE CLICK
        if (Input.GetMouseButtonDown(0))
        {
            //DATA
            GameObject unitTile;
            GameObject[] n;

            //CHANGING UNITS (IF SOME UNIT IS ALREADY SELECTED)
            if (ourHitObject.GetComponent<Unit>() != selectedUnit && selectedUnit != null)
            {
                unitTile = selectedUnit.getHouse();
                n = unitTile.GetComponentInParent<Hex>().getNeighbors();
                colorNeighbors(Color.white, unitTile, n); //REVERT COLORS OF PREVIOUS TILE NEIGHBORS
            }

            //GET AVAILABLE SPACES OF THE SELECTED UNIT
            selectedUnit = ourHitObject.GetComponent<Unit>();
            unitTile = ourHitObject.GetComponent<Unit>().getHouse();
            n = unitTile.GetComponentInParent<Hex>().getNeighbors();

            //CANT MOVE TO OCCUPIED TILE
            for (int i = 0; i < 6; i++)
            {
                if (n[i] != null)
                {
                    if (n[i].GetComponent<Hex>().getResident() != null)
                    {
                        n[i] = null;
                    }
                }

            }

            //COLOR THE AVAILABLE SPACES
            if (ourHitObject.gameObject.tag == "BKing")
            {
              colorNeighbors(Color.cyan, unitTile, n);
            }
            if (ourHitObject.gameObject.tag == "RKing")
            {
              colorNeighbors(Color.red, unitTile, n);
            }
        }

        //DESELECT UNIT ON A RIGHT CLICK
        if (Input.GetMouseButtonDown(1))
        {
            if (ourHitObject.GetComponent<Unit>() == selectedUnit)
            {
                selectedUnit = null;
                GameObject unitTile = ourHitObject.GetComponent<Unit>().getHouse();
                GameObject[] n = unitTile.GetComponentInParent<Hex>().getNeighbors();
                colorNeighbors(Color.white, unitTile, n);
            }
        }
    }

    void colorNeighbors(Color c, GameObject unitTile, GameObject[] n)
    {
        MeshRenderer mr;

        if (n[0] != null)
        {
            //COLOR LEFT TILE
            if (unitTile.GetComponentInParent<Hex>().x != 0)
            {
                mr = n[0].GetComponentInChildren<MeshRenderer>();
                mr.material.color = c;
            } //left
        }

        if (n[1] != null)
        {
            //COLOR RIGHT TILE
            if (unitTile.GetComponentInParent<Hex>().x != 9)
            {
                mr = n[1].GetComponentInChildren<MeshRenderer>();
                mr.material.color = c;
            } //right
        }

        if (n[2] != null)
        {
            //COLOR UPPER LEFT TILE
            if (unitTile.GetComponentInParent<Hex>().y != 11)
            {
                if (unitTile.GetComponentInParent<Hex>().y % 2 == 1)
                {
                    mr = n[2].GetComponentInChildren<MeshRenderer>();
                    mr.material.color = c;
                }
                if (unitTile.GetComponentInParent<Hex>().y % 2 == 0 && unitTile.GetComponentInParent<Hex>().x != 0)
                {
                    mr = n[2].GetComponentInChildren<MeshRenderer>();
                    mr.material.color = c;
                }
            } //upper left
        }

        if (n[3] != null)
        {
            //COLOR UPPER RIGHT TILE
            if (unitTile.GetComponentInParent<Hex>().y != 11)
            {
                if (unitTile.GetComponentInParent<Hex>().y % 2 == 0)
                {
                    mr = n[3].GetComponentInChildren<MeshRenderer>();
                    mr.material.color = c;
                }
                if (unitTile.GetComponentInParent<Hex>().y % 2 == 1 && unitTile.GetComponentInParent<Hex>().x != 9)
                {
                    mr = n[3].GetComponentInChildren<MeshRenderer>();
                    mr.material.color = c;
                }
            } //upper right
        }

        if (n[4] != null)
        {
            //COLOR LOWER LEFT TILE
            if (unitTile.GetComponentInParent<Hex>().y != 0)
            {
                if (unitTile.GetComponentInParent<Hex>().y % 2 == 1)
                {
                    mr = n[4].GetComponentInChildren<MeshRenderer>();
                    mr.material.color = c;
                }
                if (unitTile.GetComponentInParent<Hex>().y % 2 == 0 && unitTile.GetComponentInParent<Hex>().x != 0)
                {
                    mr = n[4].GetComponentInChildren<MeshRenderer>();
                    mr.material.color = c;
                }
            } //lower left
        }

        if (n[5] != null)
        {
            //COLOR LOWER RIGHT TILE
            if (unitTile.GetComponentInParent<Hex>().y != 0)
            {
                if (unitTile.GetComponentInParent<Hex>().y % 2 == 0)
                {
                    mr = n[5].GetComponentInChildren<MeshRenderer>();
                    mr.material.color = c;
                }
                if (unitTile.GetComponentInParent<Hex>().y % 2 == 1 && unitTile.GetComponentInParent<Hex>().x != 9)
                {
                    mr = n[5].GetComponentInChildren<MeshRenderer>();
                    mr.material.color = c;
                }
            } //lower right
        }
    }
}
