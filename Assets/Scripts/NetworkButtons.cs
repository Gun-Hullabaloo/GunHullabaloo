using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkButtons : MonoBehaviour
{
    private RelayManager netManager;
    public GameObject DebugButton;

    string joinCode;

    void Awake()
    {
        netManager = GetComponent<RelayManager>();
    }
    public async void Host()
    {
        string joinCode = await netManager.createRelay();
        GameObject Join = DebugButton.transform.GetChild(1).gameObject;
        Join.GetComponentInChildren<Text>().text = joinCode;
        Join.GetComponent<Button>().interactable = false;
        // Disable();
    }
    public void setJoinCode(string input)
    {
        joinCode = input;
    }
    public void Client()
    {
        netManager.joinRelay(joinCode);
        // Disable();
    }
    void Disable()
    {
        DebugButton.SetActive(false);
    }
}