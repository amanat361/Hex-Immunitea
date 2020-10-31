using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class shop : MonoBehaviour
{
    public GameObject MouseMgr;
    private int money = 99;
    public Text moneyText;

    public void Update()
    {
        moneyText.text = "Points: " + money;
    }

    public void buySomething(GameObject unitBought)
    {
        if (money >= 3 && MouseMgr.GetComponent<MouseManager>().selectedHex != null)
        {
            money -= 3;
            int x = MouseMgr.GetComponent<MouseManager>().selectedHex.GetComponent<Hex>().x;
            int y = MouseMgr.GetComponent<MouseManager>().selectedHex.GetComponent<Hex>().y;
            float xPos;
            float yPos;

            //FIND A HEX TO PLACE THE UNIT ON
            GameObject hex_go = GameObject.Find("Hex_" + x + "_" + y);

            //GET POSITION OF THE HEX (X,Z)
            xPos = hex_go.transform.GetChild(0).transform.position.x;
            yPos = hex_go.transform.GetChild(0).transform.position.z;

            //INSTANTIATE THE UNIT AT THAT POSITION WITH HOVER
            GameObject unit_go = (GameObject)Instantiate(unitBought, new Vector3(xPos, 0.3f, yPos), Quaternion.identity);

            unit_go.GetComponent<Unit>().setHouse(hex_go); //set house of unit as hex
            hex_go.GetComponent<Hex>().setResident(unit_go); //set resident of hex as unit
            unit_go.name = "Pawn: (" + x + "," + y + ")"; //name the unit
            unit_go.transform.SetParent(this.transform); //keep units neatly with army
            unit_go.isStatic = true; //idk wtf this does but it's here lol
            MeshRenderer mr = hex_go.GetComponentInChildren<MeshRenderer>();
            mr.material.color = Color.white;
        }
    }
}
