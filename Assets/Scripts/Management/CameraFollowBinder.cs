using UnityEngine;
using Cinemachine;

public class CameraFollowBinder : MonoBehaviour
{
    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found in scene!");
            return;
        }

        
        CinemachineVirtualCamera[] virtualCams = GetComponentsInChildren<CinemachineVirtualCamera>();

        foreach (var cam in virtualCams)
        {
            cam.Follow = player.transform;
        }

        Debug.Log("Cinemachine Follow targets set to Player.");
    }
}
