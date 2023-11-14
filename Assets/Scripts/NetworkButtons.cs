using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkButtons : MonoBehaviour
{
    private RelayManager netManager;
    public GameObject DebugButton;

    GameObject Create;
    GameObject Join;
    GameObject JoinInput;
    GameObject PlayerName;

    string joinCode;

    void Awake()
    {
        netManager = GetComponent<RelayManager>();

        Create = DebugButton.transform.GetChild(0).gameObject;
        Join = DebugButton.transform.GetChild(1).gameObject;
        JoinInput = DebugButton.transform.GetChild(2).gameObject;
        PlayerName = DebugButton.transform.GetChild(3).gameObject;
    }
    public async void RelayHost()
    {
        string joinCode = await netManager.createRelay();
        PlayerName.GetComponent<Text>().text = netManager.getPlayerName();

        Create.GetComponent<Button>().interactable = false;
        Join.GetComponent<Button>().interactable = false;
        Join.GetComponentInChildren<Text>().text = joinCode;
        JoinInput.SetActive(false);

        // Disable();
    }
    public void setJoinCode(string input)
    {
        joinCode = input;
    }
    public void RelayJoin()
    {
        netManager.joinRelay(joinCode);
        PlayerName.GetComponent<Text>().text = netManager.getPlayerName();

        Create.GetComponent<Button>().interactable = false;
        Join.GetComponent<Button>().interactable = false;
        JoinInput.SetActive(false);

        // Disable();
    }
    void Disable()
    {
        DebugButton.SetActive(false);
    }
}