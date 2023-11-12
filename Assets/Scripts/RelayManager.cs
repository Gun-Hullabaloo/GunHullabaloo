using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task<string> createRelay()
    {
        try
        {
            // DebugController.consoleOutput("Allocating Relay Server");
            Allocation alloc = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
            // DebugController.consoleOutput("Relay Server creation successful with Room " + joinCode);

            // DebugController.consoleOutput("Initiating Relay Server");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                alloc.RelayServer.IpV4,
                (ushort)alloc.RelayServer.Port,
                alloc.AllocationIdBytes,
                alloc.Key,
                alloc.ConnectionData
            );
            // DebugController.consoleOutput("Initiation successful");

            // DebugController.consoleOutput("Starting Host");
            NetworkManager.Singleton.StartHost();

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
        return "";
    }

    public async void joinRelay(string joinCode)
    {
        try
        {
            // DebugController.consoleOutput("Joining Room " + joinCode);
            JoinAllocation joinAlloc = await RelayService.Instance.JoinAllocationAsync(joinCode);
            // DebugController.consoleOutput("Join successful");

            // DebugController.consoleOutput("Initiating Relay Client");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAlloc.RelayServer.IpV4,
                (ushort)joinAlloc.RelayServer.Port,
                joinAlloc.AllocationIdBytes,
                joinAlloc.Key,
                joinAlloc.ConnectionData,
                joinAlloc.HostConnectionData
            );
            // DebugController.consoleOutput("Initiation successful");

            // DebugController.consoleOutput("Starting Client");
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
