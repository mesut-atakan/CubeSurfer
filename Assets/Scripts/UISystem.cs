using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UISystem : MonoBehaviour
{
    private int EXP;

    public TextMeshProUGUI expText;

    public TextMeshProUGUI menuExpText;

    public GameObject MenuUI;

    public ushort start;



    private void Start() {
        Application.targetFrameRate = 60;
    }


    public void MenuSystem()
    {
        Time.timeScale = 0;
        expText.enabled = false;
        MenuUI.SetActive(true);
        scoreText(EXP);
        Application.targetFrameRate = 30;
    }



    public void ExpSystem(int index)
    {
        EXP += index;
        expText.text = EXP.ToString();
    }



    public void StartButton()
    {
        start = 1;
        PlayerPrefs.SetInt("start", start);
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Application.targetFrameRate = 60;
    }


    public void ExitButton()
    {
        start = 0;
        PlayerPrefs.SetInt("start", start);
        Application.Quit();
    }


    public void scoreText(int score)
    {
        menuExpText.text = "Score:" + "\n" + score;
    }
}
