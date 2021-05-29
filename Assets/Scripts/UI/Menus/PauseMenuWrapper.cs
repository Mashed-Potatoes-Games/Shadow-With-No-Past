using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PauseMenuWrapper : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        InputControls.CancelButton.Add(Pause);
    }
    public void Pause()
    {
        Time.timeScale = 0;
        Debug.Log("GamePaused");
        InputControls.CancelButton.Remove(Pause);
        InputControls.CancelButton.AddInterrupting(Resume);
        animator.SetBool("MenuOpened", true);
    }

    public bool Resume()
    {
        Time.timeScale = 1;
        Debug.Log("GameUnpaused");
        InputControls.CancelButton.Remove(Resume);
        InputControls.CancelButton.Add(Pause);
        animator.Play("FadeOut");
        animator.SetBool("MenuOpened", false);
        return true;
    }

    public void FadeIsOver()
    {

    }
}
