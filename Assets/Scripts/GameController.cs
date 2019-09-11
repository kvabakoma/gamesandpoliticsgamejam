using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private GameObject DudePrefab, ShipContainer, DudeSpawningPoint, DudesContainer;
    public List<GameObject> DudesList;

    [SerializeField]
    int NumberOfPlayers;

    [SerializeField]
    private Texture2D[] textures;

    [SerializeField]
    private float shipMovementSpeed = .01f;

    private int shirtId;

    // Start is called before the first frame update
    void Start()
    {
        DudesList = new List<GameObject>();
        shirtId = 0;
        SpanwNewDude();
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
        if (shirtId > 0 && shirtId % 3 == 0) Dude.GetComponent<DudeController>().badGuy = true;
        else Dude.GetComponent<DudeController>().badGuy = false;
        shirtId++;
        if (shirtId > 9) shirtId = 0;
        Dude.transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", textures[shirtId]);
        DudesList.Add(Dude);

        if (DudesList.Count < NumberOfPlayers) Invoke("SpanwNewDude", 10);
    }
}
