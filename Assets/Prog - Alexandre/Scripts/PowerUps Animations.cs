using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUpsAnimations : MonoBehaviour
{
    [SerializeField] private Animator powerAnimator;
    [SerializeField] private Animation blackBackAnim;
    [SerializeField] private Animation cadreAnim;
    [SerializeField] private Animation textAnim;

    [SerializeField] private string textPOW1;
    [SerializeField] private string textPOW2;
    [SerializeField] private string textPOW3;

    public void StartAnimPOW(int nbr)
    {
        switch (nbr)
        {
            case 1:
                blackBackAnim.Play();
                cadreAnim.Play();
                powerAnimator.SetTrigger("Hand");
                textAnim.GetComponent<TextMeshProUGUI>().text = textPOW1;
                textAnim.Play();
                powerAnimator.SetTrigger("Sword1");
                break;
            case 2:
                blackBackAnim.Play();
                cadreAnim.Play();
                powerAnimator.SetTrigger("Head");
                textAnim.Play();
                powerAnimator.SetTrigger("Sword2");
                textAnim.GetComponent<TextMeshProUGUI>().text = textPOW2;
                break;
            case 3:
                blackBackAnim.Play();
                cadreAnim.Play();
                powerAnimator.SetTrigger("Heart");
                textAnim.Play();
                powerAnimator.SetTrigger("Sword3");
                textAnim.GetComponent<TextMeshProUGUI>().text = textPOW3;
                break;
        }
    }
}
