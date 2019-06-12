﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public GameObject message;

    public ShopItem[] shopItems;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        if (PlayerPrefs.HasKey("openCount"))
        {
            if (PlayerPrefs.GetString("lastOpenDate") != System.DateTime.Today.ToLongDateString())
            {
                GameObject confirm = Instantiate(message, GameObject.Find("Canvas").transform);
                confirm.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Daily Counters!";
                confirm.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = "Thanks for logging in! Here is 125 counters!";
                confirm.GetComponent<Animator>().SetTrigger("Entry");
                PlayerPrefs.SetInt("Counters", PlayerPrefs.GetInt("Counters") + 125);
                FindObjectOfType<CreditsDisplay>().UpdateAmount();
            }
            PlayerPrefs.SetString("lastOpenDate", System.DateTime.Today.ToLongDateString());
            PlayerPrefs.SetInt("openCount", PlayerPrefs.GetInt("openCount") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("openCount", 1);

            int id = Random.Range(0, shopItems.Length);

            PlayerPrefs.SetInt("Owned " + ShopItemType.Color.ToString() + " number " + id.ToString(), 1);
            PlayerPrefs.SetInt(ShopItemType.Color.ToString() + "selected", id);

            GameObject.Find("LoadingPlayer").GetComponent<ColourSetterLoad>().SetColor(shopItems[id].foregroundColor);
            foreach (SyncName syncName in FindObjectsOfType<SyncName>())
            {
                syncName.UpdateColor();
            }
        }
    }
}