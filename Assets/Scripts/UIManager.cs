using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    private TMP_Text text;

    void Start()
    {
    }
    void OnEnable() 
    {
        text = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "Welcome back! You have " + GlobalData.Instance.pearls + " pearls.\nDo you want to buy something?";
        dialogBox.SetActive(true);

    }
    public void ClosePanel()
    {
        dialogBox.SetActive(false);
    }
}
