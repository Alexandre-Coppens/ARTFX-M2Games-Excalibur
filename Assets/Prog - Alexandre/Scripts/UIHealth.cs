using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    [SerializeField] private Sprite life3;
    [SerializeField] private Sprite life2;
    [SerializeField] private Sprite life1;
    [SerializeField] private Sprite life0;

    public static UIHealth instance;
    private void Awake()
    {
        if(instance == null) { instance = this; }
    }

    public void UpdateHealth(int health)
    {
        switch (health)
        {
            case 0:
                GetComponent<Image>().sprite = life0; 
                break;

            case 1:
                GetComponent<Image>().sprite = life1;
                break;

            case 2:
                GetComponent<Image>().sprite = life2;
                break;

            case 3:
                GetComponent<Image>().sprite = life3;
                break;
        }
    }
}
