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

    // Start is called before the first frame update
    void Start()
    {
        DudesList = new List<GameObject>();


        SpanwNewDude();
    }

    private void SpanwNewDude()
    {
        DudesList.Add(Instantiate(DudePrefab, new Vector3(Random.Range(1, 3), 0, Random.Range(-3, 3)), Quaternion.identity));

        if (DudesList.Count < NumberOfPlayers) Invoke("SpanwNewDude", 10);
    }
}
