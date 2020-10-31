using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Vector3 destination; //where to go
    public Vector3 pos; //current position
    public GameObject house; //tile the unit is on
    public float y0, randY, randS;
    public char team;

    void Start()
    {
        y0 = transform.position.y;
        transform.Rotate(-90,0,0);
        randY = Random.Range(0.08f,0.13f);
        randS = Random.Range(3.0f,3.4f);
    }

    void Update()
    {
      pos.x = transform.position.x;
      pos.z = transform.position.z;
      pos.y = y0 + randY * Mathf.Sin(randS * Time.time);
      transform.position = pos;
    }

    public Unit moveTo(GameObject hex)
    {
        house = hex.transform.parent.gameObject;
        destination = hex.transform.position;
        pos.x += destination.x - pos.x;
        pos.z += destination.z - pos.z;
        pos.y = 0.3f;
        transform.position = pos;
        return null;
    }

    public GameObject getHouse()
    {
        return house;
    } //getter (house)

    public void setHouse(GameObject h)
    {
        house = h;
    } //setter (house)
}
