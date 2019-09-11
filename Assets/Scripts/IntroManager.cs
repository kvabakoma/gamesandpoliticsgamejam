using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{

    public GameObject introUI;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("ShowUI", 6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowUI()
    {
        introUI.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("HumanSandbox");
    }
}
