using UnityEngine;

public class IntroDialogueStarter : MonoBehaviour
{
    public GameObject introPopupPrefab;

    private void Start()
    {
        
        GameObject popup = Instantiate(introPopupPrefab);
        popup.GetComponent<IntroPopup>().SetMessage(
            "Welcome, traveler! Use WASD to move, press E to talk. Explore the world and uncover its stories."
        );

        
        popup.GetComponent<IntroPopup>().continueButton.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }
}
