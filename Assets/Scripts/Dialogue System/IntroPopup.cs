using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] public Button continueButton;

    private void Start()
    {
        continueButton.onClick.AddListener(ClosePopup);
    }

    public void SetMessage(string message)
    {
        messageText.text = message;
    }

    private void ClosePopup()
    {
        Destroy(gameObject);
    }
}
