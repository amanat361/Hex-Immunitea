using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MouseManager : MonoBehaviour
{
    //AUDIO
    public AudioSource source;
    public AudioClip unitSelectSound;
    public AudioClip unitDeselectSound;
    public AudioClip gameWinSound;
    public AudioClip enemyInRangeSound;
    public AudioClip enemyDeathSound;

    //CAMERA
    public Camera cam1;
    public Camera cam2;

    //PLAYER
    public char turn;
    public Unit selectedUnit;
    public Hex selectedHex;
    public bool attacked = false;

    //SETUP CAMERAS
    void Start()
    {
      turn = 'A';
      cam1.enabled = true;
      cam2.enabled = false;
    }

    //CHECK WHERE THE MOUSE IS HOVERING OVER EVERY FRAME
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
            if (ourHitObject.GetComponent<Unit>() != null) MouseOver_Unit(ourHitObject);
            //IF OBJECT HAS HEX SCRIPT, RUN THE HEX FUNCTION
            else if (ourHitObject.GetComponent<HexBody>() != null) MouseOver_Hex(ourHitObject);
        }
    }

    //IF MOUSE WAS OVER A HEX (e.g. Clicked on a Hex)
    void MouseOver_Hex(GameObject ourHitObject)
    {
        //DATA
        GameObject[] n;

        //USER LEFT CLICK ON HEX
        if (Input.GetMouseButtonDown(0) || attacked)
        {
            if (attacked) attacked = false;
            //IF A UNIT IS SELECTED CHECK IF UNIT CAN MOVE TO THAT HEX
            if (selectedUnit != null)
            {
                //UNIT BY DEFAULT CAN'T MOVE TO THAT HEX
                bool canMoveToTile = false;

                //MAKE AN ARRAY OF THE NEIGHBORING HEXES OF UNIT (PLACES IT CAN MOVE)
                //Saving Variable Name Lenth by putting repetitive text in one variable
                Hex preN = selectedUnit.GetComponent<Unit>().getHouse().GetComponentInChildren<Hex>();
                if (selectedUnit.gameObject.tag == "king") n = preN.getKingSpaces();
                else if (selectedUnit.gameObject.tag == "bishop") n = preN.getBishopSpaces();
                else if (selectedUnit.gameObject.tag == "rook") n = preN.getRookSpaces();
                else n = preN.getPawnSpaces(selectedUnit.team);

                //IF THE SELECTED TILE IS AN AVAILABLE HEX AND THE DESIRED TARGED HEX, CAN MOVE = TRUE
                for (int i = 0; i < n.Length; i++)
                {
                    if (n[i] != null) //Because the neighbor array is fixed at 22
                    //so this excludes the unused space within the neighbor array
                    {
                        if (n[i].gameObject == ourHitObject.transform.parent.gameObject) {
                            canMoveToTile = true;
                            break;
                        } //If available tile is the tile requested to be moved to
                    } //If there is no current unit on the new tile
                } //For

                //IF CAN MOVE:
                //UNCOLOR AVAILABLE TILES
                //REMOVE LINK BETWEEN UNIT AND CURRENT HEX
                //MOVE UNIT TO NEW HEX
                //ADD LINK BETWEEN UNIT AND THE NEW HEX
                //DESELECT THE UNIT
                //IF ENEMY PRESENT ON NEW HEX, DESTROY ENEMY
                if (canMoveToTile)
                {
                  GameObject temp = ourHitObject.GetComponentInParent<Hex>().getResident();
                  if (temp == null || temp.GetComponentInParent<Unit>().team != selectedUnit.team)
                  {
                    colorAvailableHexes(Color.white, selectedUnit.getHouse().gameObject, n);
                    selectedUnit.getHouse().GetComponent<Hex>().setResident(null);
                    if (temp != null)
                    {
                      if (temp.GetComponentInParent<Unit>().tag == "king")
                      {
                        Destroy(temp);
                        StartCoroutine(gameOver()); //end game
                      }
                      else
                      {
                        Destroy(temp); //just kill the thing
                        StartCoroutine(enemyKilledSoundFunc());
                      }
                    }
                    selectedUnit.moveTo(ourHitObject);
                    ourHitObject.GetComponentInParent<Hex>().setResident(selectedUnit.transform.gameObject);
                    selectedUnit = null;

                    //Play Deselect unitSelectSound
                    source.clip = unitDeselectSound;
                    source.Play();

                    //switch up who's turn it is and change the cameras
                    if (turn == 'A') turn = 'B';
                    else turn = 'A';
                    cam1.enabled = !cam1.enabled;
                    cam2.enabled = !cam2.enabled;
                  }
                }
            }
        }
    }

    //IF MOUSE WAS OVER A UNIT (e.g. Clicked on a Unit)
    void MouseOver_Unit(GameObject ourHitObject)
    {
        //DATA
        GameObject unitTile = ourHitObject.GetComponent<Unit>().getHouse(); //The Hex GAMEOBJECT of the Unit
        Hex preN; //Save variable name space
        GameObject[] n; //Array of available spaces to move to
        bool rangeSoundCheck = false; //Doesn't play the range sound for multiple enemies, just the first

        //SELECT UNIT ON LEFT MOUSE CLICK
        if (Input.GetMouseButtonDown(0))
        {
            //IF UNIT IS SELECTED ON ONE TEAM AND THEN A UNIT FROM THE OTHER TEAM IS CLICKED ON TO BE KILLED
            if (selectedUnit != null && selectedUnit.team != ourHitObject.GetComponentInChildren<Unit>().team)
            {
              attacked = true; //allow hex function to be called without hex being clicked on
              MouseOver_Hex(unitTile.transform.GetChild(0).gameObject); // fix this shit tomorrow
            }

            else if (ourHitObject.GetComponentInChildren<Unit>().team == turn)
            {
              //CHANGE SELECTED UNIT IF UNIT ALREADY SELECTED
              if (ourHitObject.GetComponent<Unit>() != selectedUnit && selectedUnit != null)
              {
                //Get posible tile moves array dependent on the piece selected
                preN = selectedUnit.getHouse().GetComponentInParent<Hex>(); //Get the Hex from the selected unit
                if (selectedUnit.tag == "king") n = preN.getKingSpaces(); //Get available king moves for that hex
                else if (selectedUnit.tag == "bishop") n = preN.getBishopSpaces(); //Get available bishop moves
                else if (selectedUnit.tag == "rook") n = preN.getRookSpaces(); //Same thing for the rook
                else n = preN.getPawnSpaces(selectedUnit.team); //Little pawn boi
                colorAvailableHexes(Color.white, unitTile, n);
              }

              //Get posible tile moves array dependent on the piece selected
              selectedUnit = ourHitObject.GetComponent<Unit>(); //Switch to the new selected piece
              //Play unitSelectSound
              source.clip = unitSelectSound;
              source.Play();
              preN = selectedUnit.getHouse().GetComponentInParent<Hex>(); //Get the Hex from the selected unit
              if (selectedUnit.tag == "king") n = preN.getKingSpaces(); //Get available king moves for that hex
              else if (selectedUnit.tag == "bishop") n = preN.getBishopSpaces(); //Get available bishop moves
              else if (selectedUnit.tag == "rook") n = preN.getRookSpaces(); //Same thing for the rook
              else n = preN.getPawnSpaces(selectedUnit.team); //Little pawn boi

              //DONT COLOR OCCUPIED TILE
              for (int i = 0; i < n.Length; i++)
              {
                  if (n[i] != null) //Because the neighbor array is fixed at 22
                  //so this excludes the unused space within the neighbor array
                  {
                      if (n[i].GetComponent<Hex>().getResident() != null)
                      {
                          if (n[i].GetComponent<Hex>().getResident().GetComponent<Unit>().team != selectedUnit.team)
                          {
                            MeshRenderer mr = n[i].GetComponentInChildren<MeshRenderer>();
                            mr.material.color = Color.red;
                            rangeSoundCheck = true;
                          }
                          n[i] = null;
                      }
                  }
              }

              //Play unit in range sound
              if (rangeSoundCheck)
              {
                source.clip = enemyInRangeSound;
                source.Play();
                rangeSoundCheck = false;
              }

              colorAvailableHexes(Color.cyan, unitTile, n);
            }
        }

        //DESELECT UNIT ON A RIGHT CLICK
        if (Input.GetMouseButtonDown(1) && ourHitObject.GetComponent<Unit>() == selectedUnit)
        {
          //Play unitDeselectSound
          source.clip = unitDeselectSound;
          source.Play();

          //Get posible tile moves array dependent on the piece selected
          preN = selectedUnit.getHouse().GetComponentInParent<Hex>(); //Get the Hex from the selected unit
          if (selectedUnit.tag == "king") n = preN.getKingSpaces(); //Get available king moves for that hex
          else if (selectedUnit.tag == "bishop") n = preN.getBishopSpaces(); //Get available bishop moves
          else if (selectedUnit.tag == "rook") n = preN.getRookSpaces(); //Same thing for the rook
          else n = preN.getPawnSpaces(selectedUnit.team); //Little pawn boi
          colorAvailableHexes(Color.white, unitTile, n);
          selectedUnit = null;
        }
    }

    void colorAvailableHexes(Color c, GameObject unitTile, GameObject[] n)
    {
        MeshRenderer mr;

        for (int x = 0; x < n.Length; x++)
        {
            if (n[x] != null)
            {
                mr = n[x].GetComponentInChildren<MeshRenderer>();
                mr.material.color = c;
            }
        }
    }

    IEnumerator enemyKilledSoundFunc()
    {
      yield return new WaitWhile (()=> source.isPlaying);
      source.clip = enemyDeathSound;
      source.Play();
      yield return new WaitWhile (()=> source.isPlaying);
    }

    IEnumerator gameOver()
    {
      yield return new WaitWhile (()=> source.isPlaying);
      source.clip = gameWinSound;
      source.Play();
      yield return new WaitWhile (()=> source.isPlaying);
      SceneManager.LoadScene(0); //end game
    }
}
