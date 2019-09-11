using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private GameObject DudePrefab;
    public List<GameObject> DudesList;

    [SerializeField]
    int NumberOfPlayers;

    [SerializeField]
    private Texture2D[] textures;

    private int shirtId;

    // Start is called before the first frame update
    void Start()
    {
        DudesList = new List<GameObject>();
        shirtId = 0;
        SpanwNewDude();
    }

    private void SpanwNewDude()
    {
        GameObject Dude = Instantiate(DudePrefab, new Vector3(Random.Range(1, 3), 0, Random.Range(-3, 3)), Quaternion.identity);
        shirtId++;
        if (shirtId > 9) shirtId = 0;
        Dude.transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", textures[shirtId]);
        DudesList.Add(Dude);

        if (DudesList.Count < NumberOfPlayers) Invoke("SpanwNewDude", 10);
    }
}
