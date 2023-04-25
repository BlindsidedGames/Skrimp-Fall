using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuOpenClose : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Button openCloseButton;
    private static readonly int Open = Animator.StringToHash("Open");

    private void Start()
    {
        openCloseButton.onClick.AddListener(OpenCloseMenu);
    }


    private void OpenCloseMenu()
    {
        var open = "Open";
        animator.SetBool(Open, !animator.GetBool(Open));
    }
}