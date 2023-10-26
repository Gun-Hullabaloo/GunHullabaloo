using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkButtons : MonoBehaviour
{
    private NetworkManager netManager;
    public GameObject DebugButton;

    void Awake()
    {
        netManager = GetComponent<NetworkManager>();
    }
    public void Host()
    {
        netManager.StartHost();
        Disable();
    }
    public void Server()
    {
        netManager.StartServer();
        Disable();
    }
    public void Client()
    {
        netManager.StartClient();
        Disable();
    }
    void Disable()
    {
        DebugButton.SetActive(false);
    }
}