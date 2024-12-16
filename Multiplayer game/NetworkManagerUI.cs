using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(() => { 
        NetworkManager.Singleton.StartHost();
        });

        joinButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }

}
