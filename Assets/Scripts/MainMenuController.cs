using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Controls Story Selection, displays Keys for Chapters, Tokens for inGame purchasables 
public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public Button startButton;
    void Start()
    {
        startButton.onClick.AddListener(StartButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartButtonClick()
    {
        SceneManager.LoadScene("GameScene1");
    }
}
