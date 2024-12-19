using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public bool canInteractController = false;

    private int currentChoice = 0;
    private bool wait;
    private Player_Inputs playerInputs;
    private List<string> buttons = new List<string>();

    public Button start;
    public Button restart;
    public Button resume;
    public Button menu;
    public Button quit;
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0) canInteractController=true;
        playerInputs = Player_Inputs.instance;
        if(start != null )      { buttons.Add("start");     start.GetComponent<Button>().onClick.AddListener(StartFunction); }
        if(restart != null )    { buttons.Add("restart");   restart.GetComponent<Button>().onClick.AddListener(RestartFunction); }
        if(resume != null )     { buttons.Add("resume");    resume.GetComponent<Button>().onClick.AddListener(ResumeFunction); }
        if(menu != null )       { buttons.Add("menu");      menu.GetComponent<Button>().onClick.AddListener(MenuFunction); }
        if(quit != null )       { buttons.Add("quit");      quit.GetComponent<Button>().onClick.AddListener(QuitFunction); }
    }

    // Update is called once per frame
    void Update()
    {
        if(canInteractController)
        {
            if(playerInputs == null) playerInputs = Player_Inputs.instance;
            if (playerInputs.attackPressed)
            {
                switch (buttons[currentChoice - 1])
                {
                    case "start":
                        StartFunction();
                        break;
                    case "restart":
                        RestartFunction();
                        break;
                    case "resume":
                        ResumeFunction();
                        break;
                    case "menu":
                        MenuFunction();
                        break;
                    case "quit":
                        QuitFunction();
                        break;
                }
            }

            if (playerInputs.menu > 0.5f && !wait)
            {
                if(currentChoice == 0) { currentChoice = 1; wait = true; }
                else
                {
                    currentChoice = Mathf.Clamp(currentChoice - 1, 1, buttons.Count) ;
                    wait = true;
                }
            }
            if (playerInputs.menu < -0.5f && !wait)
            {
                if (currentChoice == 0) { currentChoice = 1; wait = true; }
                else
                {
                    currentChoice = Mathf.Clamp(currentChoice + 1, 1, buttons.Count);
                    wait = true;
                }
            }
            if(playerInputs.menu < 0.25f && playerInputs.menu > -0.25f) wait = false;

            if(currentChoice == 0) return;
            if(buttons[currentChoice - 1] == "start")   start.Select();
            if(buttons[currentChoice - 1] == "restart") restart.Select();
            if(buttons[currentChoice - 1] == "resume")  resume.Select();
            if(buttons[currentChoice - 1] == "menu")    menu.Select();
            if(buttons[currentChoice - 1] == "quit")    quit.Select();
        }
        else currentChoice = 0;
    }


 public   void StartFunction()
    {
        if(!canInteractController) { return; }
        SceneManager.LoadScene(1);
    }
 public   void RestartFunction()
    {
        if (!canInteractController) { return; }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ResumeFunction()
    {
        if (!canInteractController) { return; }
    }

  public void MenuFunction() 
    {
        if (!canInteractController) { return; }
        SceneManager.LoadScene(0);
    }

  public void QuitFunction() 
    {
        if (!canInteractController) { return; }
        Application.Quit();
    }

}
