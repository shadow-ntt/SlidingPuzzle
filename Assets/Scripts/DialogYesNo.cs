using System;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization;
using TMPro;

public class DialogYesNo : MonoBehaviour
{
    public static DialogYesNo Instance { get; private set; }
    [SerializeField] private Button btnYes;
    [SerializeField] private Button btnNo;
    [SerializeField] private TextMeshProUGUI content;
    private Action yesAction;
    private Action noAction;

    private void Awake()
    {
        Debug.Log("created");
        btnYes.onClick.AddListener(OnYes);
        btnNo.onClick.AddListener(OnNo);
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show(string message, Action onYes = null, Action onNo = null)
    {
        yesAction = onYes;
        noAction = onNo;
        content.text = message;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnYes()
    {
        yesAction?.Invoke();
        Hide();
    }

    private void OnNo()
    {
        noAction?.Invoke();
        Hide();
    }
}