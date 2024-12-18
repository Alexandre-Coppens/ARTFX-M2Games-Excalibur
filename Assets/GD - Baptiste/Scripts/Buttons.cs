using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public Button start;
    public Button resume;
    public Button menu;
    public Button quit;
    // Start is called before the first frame update
    void Start()
    {
        start.GetComponent<Button>().onClick.AddListener(StartFunction);
        resume.GetComponent<Button>().onClick.AddListener(ResumeFunction);
        menu.GetComponent<Button>().onClick.AddListener(MenuFunction);
        quit.GetComponent<Button>().onClick.AddListener(QuitFunction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


 public   void StartFunction()
    {
        SceneManager.LoadScene(1);
    }
    public void ResumeFunction()
    {
    }

  public void MenuFunction() 
    {
        SceneManager.LoadScene(0);
    }

  public void QuitFunction() 
    {
    Application.Quit();
    }

}
