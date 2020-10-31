using System.Collections;
using UnityEngine;

public class ArmyGenerator : MonoBehaviour
{
    public GameObject blueKing;
    public GameObject blueBishop;
    public GameObject redKing;
    public GameObject redBishop;
    public GameObject blueRook;
    public GameObject redRook;
    public GameObject bluePawn;
    public GameObject redPawn;

    public void Start() //change to void start when you want to use this script again
    {
      //DEFAULT UNIT LOADOUT FOR BLUE PAWNS
      spawnUnits(bluePawn, "Blue Pawn", 1, 0, true); //spawn one pawn on each column
      spawnUnits(bluePawn, "Blue Pawn", 1, 1, true); //the row is 2nd closest to base
      spawnUnits(bluePawn, "Blue Pawn", 1, 2, true); //same for blue and red teams

      //DEFAULT UNIT LOADOUT FOR RED PAWNS
      spawnUnits(redPawn, "Red Pawn", 8, 0, true);
      spawnUnits(redPawn, "Red Pawn", 8, 1, true);
      spawnUnits(redPawn, "Red Pawn", 8, 2, true);

      //DEFAULT UNIT LOADOUT FOR BLUE BISHOPS
      spawnUnits(blueBishop, "Blue Bishop", 0, 1, false); //two bishops between each set
      spawnUnits(blueBishop, "Blue Bishop", 0, 2, false); //of rooks, again, same for both
      spawnUnits(blueBishop, "Blue Bishop", 0, 6, false); //teams, red and blue alike
      spawnUnits(blueBishop, "Blue Bishop", 0, 7, false);

      //DEFAULT UNIT LOADOUT FOR RED BISHOPS
      spawnUnits(redBishop, "Red Bishop", 9, 1, false);
      spawnUnits(redBishop, "Red Bishop", 9, 2, false);
      spawnUnits(redBishop, "Red Bishop", 9, 6, false);
      spawnUnits(redBishop, "Red Bishop", 9, 7, false);

      //DEFAULT UNIT LOADOUT FOR BLUE ROOKS
      spawnUnits(blueRook, "Blue Rook", 0, 0, false); //one rook on each corner and one
      spawnUnits(blueRook, "Blue Rook", 0, 8, false); //on each side of the king, they
      spawnUnits(blueRook, "Blue Rook", 0, 3, false); //gotta protect their royalty!
      spawnUnits(blueRook, "Blue Rook", 0, 5, false);

      //DEFAULT UNIT LOADOUT FOR RED ROOKS
      spawnUnits(redRook, "Red Rook", 9, 0, false);
      spawnUnits(redRook, "Red Rook", 9, 8, false);
      spawnUnits(redRook, "Red Rook", 9, 3, false);
      spawnUnits(redRook, "Red Rook", 9, 5, false);

      //DEFAULT UNIT SPAWN FOR BLUE KING
      spawnUnits(blueKing, "Blue King", 0, 4, false); //king centered on base row

      //DEFAULT UNIT SPAWN FOR RED KING
      spawnUnits(redKing, "Red King", 9, 4, false);
    }

    private void spawnUnits(GameObject unitType, string name, int row, int pos, bool auto)
    {
      if (auto)
      {
        for (int x = 0; x < 9; x++)
        {
          //DATA
          float xPos;
          float yPos;

          if (x % 3 == pos)
          {
            //FIND A HEX TO PLACE THE UNIT ON
            GameObject hex_go = GameObject.Find("Hex_" + x + "_" + row);

            //GET POSITION OF THE HEX (X,Z)
            xPos = hex_go.transform.GetChild(0).transform.position.x;
            yPos = hex_go.transform.GetChild(0).transform.position.z;

            //INSTANTIATE THE UNIT AT THAT POSITION WITH HOVER
            GameObject unit_go = (GameObject)Instantiate(unitType, new Vector3(xPos, 0.55f, yPos), Quaternion.identity);

            unit_go.GetComponent<Unit>().setHouse(hex_go); //set house of unit as hex
            hex_go.GetComponent<Hex>().setResident(unit_go); //set resident of hex as unit
            unit_go.name = name; //name the unit
            unit_go.transform.SetParent(this.transform); //keep units neatly with army
            unit_go.isStatic = true; //idk wtf this does but it's here lol
          }
        }
      }
      else
      {
        //DATA
        float xPos;
        float yPos;

        //FIND A HEX TO PLACE THE UNIT ON
        GameObject hex_go = GameObject.Find("Hex_" + pos + "_" + row);

        //GET POSITION OF THE HEX (X,Z)
        xPos = hex_go.transform.GetChild(0).transform.position.x;
        yPos = hex_go.transform.GetChild(0).transform.position.z;

        //INSTANTIATE THE UNIT AT THAT POSITION WITH HOVER
        GameObject unit_go = (GameObject)Instantiate(unitType, new Vector3(xPos, 0.55f, yPos), Quaternion.identity);

        unit_go.GetComponent<Unit>().setHouse(hex_go); //set house of unit as hex
        hex_go.GetComponent<Hex>().setResident(unit_go); //set resident of hex as unit
        unit_go.name = name; //name the unit
        unit_go.transform.SetParent(this.transform); //keep units neatly with army
        unit_go.isStatic = true; //idk wtf this does but it's here lol
      }
    }
}
