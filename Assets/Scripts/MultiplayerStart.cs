﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.LiteNetLib4Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerStart : MonoBehaviour
{
    LiteNetLib4MirrorTransport transport;

    // Start is called before the first frame update
    void Start()
    {
        transport = NetworkManager.singleton.gameObject.GetComponent<LiteNetLib4MirrorTransport>();
    }

    void CheckPorts()
    {
        foreach (ServerUIObject UIObject in SyncData.servers)
        {
            if (UIObject.port == transport.port)
            {
                transport.port = (ushort)Random.Range(2345, 2365);
                CheckPorts();
            }
        }
    }

    public void StartLevel()
    {
        //if (PlayerPrefs.HasKey("BrawlPro"))
        //{
        //FindObjectOfType<Energy>().DepleteEnergy();

        if (!NetworkServer.active)
        {
            CheckPorts();
            SyncData.serverName = SyncData.name + "s FFA Server!";
            if (NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopClient();
                NetworkManager.singleton.StartHost();
            }
            else
            {
                NetworkManager.singleton.StartHost();
            }
        }

        SyncData.isCampaign = false;

        StartCoroutine(WaitForServer());
        /*}
        else
        {
            ShowError();
        }*/
    }

    IEnumerator WaitForAd()
    {
        yield return new WaitForSeconds(0.1f);
        if (PlayerPrefs.GetInt("Energy") > 0)
        {
            if (!NetworkServer.active)
            {
                CheckPorts();
                SyncData.serverName = SyncData.name + "s FFA Server!";
                if (NetworkClient.isConnected)
                {
                    NetworkManager.singleton.StopClient();
                    NetworkManager.singleton.StartHost();
                }
                else
                {
                    NetworkManager.singleton.StartHost();
                }
            }

            SyncData.isCampaign = false;

            StartCoroutine(WaitForServer());
        }
    }

    /*private void ShowError()
    {
        GameObject confirm;
        if (GameObject.Find("NoDestroyCanvas/Message"))
        {
            confirm = GameObject.Find("NoDestroyCanvas/Message");
            confirm.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "ERROR";
            confirm.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = "You need to purchase brawl pro to unlock free for all multiplayer mode, please visit the store :). You can play campaign mode without purchasing Brawl Pro";
            confirm.GetComponent<Animator>().SetTrigger("Entry");
            GameObject.Find("OtherMinor/Close").GetComponent<UINavigation>().Switch();
            GameObject.Find("Main/Store").GetComponent<UINavigation>().Switch();
            foreach (Image image in confirm.GetComponentsInChildren<Image>())
            {
                image.enabled = true;
            }
            foreach (TMPro.TextMeshProUGUI text in confirm.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
            {
                text.enabled = true;
            }
            foreach (TMPro.TMP_Text text in confirm.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
            {
                text.enabled = true;
            }
            if (confirm.GetComponent<Image>())
            {
                confirm.GetComponent<Image>().enabled = true;
            }
            if (confirm.GetComponent<TMPro.TextMeshProUGUI>())
            {
                confirm.GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
            }
            if (confirm.GetComponent<TMPro.TMP_Text>())
            {
                confirm.GetComponent<TMPro.TMP_Text>().enabled = true;
            }
        }
    }*/

    IEnumerator WaitForServer()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Main");
        int count = 0;
        while (!GameObject.Find("LocalConnectionLobby") && count < 100)
        {
            count++;
            yield return null;
        }
        if (GameObject.Find("LocalConnectionLobby"))
        {
            GameObject.Find("LocalConnectionLobby").GetComponent<NetworkLobbyPlayer>().CmdChangeReadyState(true);
        }
    }
}
