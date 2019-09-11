using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private GameObject DudePrefab, ShipContainer, DudeSpawningPoint, DudesContainer, AngrynessBar   ;
    public List<GameObject> DudesList;

    [SerializeField]
    int NumberOfPlayers;

    [SerializeField]
    private Texture2D[] textures;

    [SerializeField]
    private float shipMovementSpeed = .01f;

    private int shirtId;
    public int averageHappiness;

    // Start is called before the first frame update
    void Start()
    {
        DudesList = new List<GameObject>();
        shirtId = 0;
        SpanwNewDude();
    }

    private void Update()
    {
        int totalLove = 0;
        int i = 0;
        while (i < DudesList.Count)
        {
            totalLove += DudesList[i].GetComponent<DudeController>().love;
            i++;
        }
        
        averageHappiness = totalLove / i;
        Debug.Log("total love:" + averageHappiness);
    }

    private void FixedUpdate()
    {
        ShipContainer.transform.Rotate(Vector3.down * shipMovementSpeed);
    }

    private void SpanwNewDude()
    {
        GameObject Dude = Instantiate(DudePrefab, DudeSpawningPoint.transform.position, Quaternion.identity);
        Dude.transform.LookAt(ShipContainer.transform.position);
        Dude.transform.parent = DudesContainer.transform;
        if (shirtId > 0 && shirtId % 4 == 0) Dude.GetComponent<DudeController>().badGuy = true;
        else Dude.GetComponent<DudeController>().badGuy = false;
        shirtId++;
        if (shirtId > 9) shirtId = 0;
        Dude.transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", textures[shirtId]);
        DudesList.Add(Dude);

        if (DudesList.Count < NumberOfPlayers) Invoke("SpanwNewDude", 8);
    }
}
