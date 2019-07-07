﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    public ShopItemType itemType;
    public int id;
    public int cost;
    public string itemName;
    public int amount;
    public ShopItem item;

    public GameObject dialogue;

    public void Start()
    {
        if (PlayerPrefs.HasKey(itemType.ToString() + "selected"))
        {
            if (PlayerPrefs.GetInt(itemType.ToString() + "selected") == id)
            {
                Select();
            }
        }
    }

    public void Buy()
    {
        if (itemType != ShopItemType.Currency)
        {
            if (PlayerPrefs.HasKey("Owned " + itemType.ToString() + " number " + id.ToString()))
            {
                DeselectAll();

                Select();
            }
            else
            {
                if (PlayerPrefs.GetInt("Counters") >= cost)
                {
                    //Display Confirm
                    GameObject confirm = Instantiate(dialogue, GameObject.Find("Canvas").transform);
                    confirm.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Purchase!";
                    confirm.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(Confirmed);
                    confirm.transform.GetChild(1).GetChild(1).GetChild(2).GetComponent<TMPro.TMP_Text>().text = "Are you sure you would like to purchase " + itemName + " for " + cost + " counters?";
                    confirm.GetComponent<Animator>().SetTrigger("Entry");
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
                else
                {
                    //Display Not enough counters message
                    GameObject confirm = Instantiate(dialogue, GameObject.Find("Canvas").transform);
                    confirm.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Purchase Fail!";
                    confirm.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(OpenIAP);
                    confirm.transform.GetChild(1).GetChild(1).GetChild(2).GetComponent<TMPro.TMP_Text>().text = "You do not have enough counters, purchase some?";
                    confirm.GetComponent<Animator>().SetTrigger("Entry");
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
            }
        }
        else
        {
            //Open Google IAP
            PurchaseIAP();
        }
        FindObjectOfType<CreditsDisplay>().UpdateAmount();
    }

    public void Confirmed()
    {
        Debug.Log("Bought " + itemType.ToString() + " number " + id.ToString());
        PlayerPrefs.SetInt("Owned " + itemType.ToString() + " number " + id.ToString(), 1);
        PlayerPrefs.SetInt("Counters", PlayerPrefs.GetInt("Counters") - cost);
        transform.GetChild(3).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Owned!";

        foreach (SetMapsAndGamemodes toggle in FindObjectsOfType<SetMapsAndGamemodes>())
        {
            Debug.Log("Updated Interactables");
            toggle.UpdateIntereact();
        }

        if (transform.GetChild(3).childCount >= 2)
        {
            Destroy(transform.GetChild(3).GetChild(1).gameObject);
        }
        transform.GetChild(3).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(170, transform.GetChild(3).GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
        transform.GetChild(3).GetChild(0).localPosition = new Vector2(0, transform.GetChild(3).GetChild(0).localPosition.y);
        GetComponent<Image>().color = Color.green;

        DeselectAll();

        Select();

        FindObjectOfType<CreditsDisplay>().UpdateAmount();
    }

    public void OpenIAP()
    {
        GameObject.Find("Store/Credits").GetComponent<DisplayStoreItems>().Switch();
    }

    public void PurchaseIAP()
    {
        //Open Google IAP

        //On purchased
        ConfirmedIAP();
    }

    public void ConfirmedIAP()
    {
        Debug.Log("Bought " + amount.ToString() + " counters!");
        PlayerPrefs.SetInt("Counters", PlayerPrefs.GetInt("Counters") + amount);
    }

    public void DeselectAll()
    {
        foreach (BuyItem item in FindObjectsOfType<BuyItem>())
        {
            if (PlayerPrefs.HasKey("Owned " + item.itemType.ToString() + " number " + item.id.ToString()))
            {
                Color color = item.gameObject.GetComponent<Image>().color;
                item.gameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.2f);
            }
        }
    }

    public void Select()
    {
        if (itemType == ShopItemType.Color || itemType == ShopItemType.Skin)
        {
            Color color = GetComponent<Image>().color;

            GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.7f);

            PlayerPrefs.SetInt(itemType.ToString() + "selected", id);

            if (itemType == ShopItemType.Skin)
            {
                SyncData.skinID = item.itemID;

                GameObject.Find("LoadingPlayer").GetComponent<SkinApply>().sprites = item.sprites;
                GameObject.Find("LoadingPlayer").GetComponent<SkinApply>().UpdateSkin();

                if (item.applyOnSprite != null)
                {
                    GameObject.Find("LoadingPlayer").GetComponent<ColourSetterLoad>().SetColor(item.applyOnSprite.foregroundColor);
                    PlayerPrefs.SetInt("Owned " + ShopItemType.Color.ToString() + " number " + item.applyOnSprite.itemID.ToString(), 1);
                    PlayerPrefs.SetInt(ShopItemType.Color.ToString() + "selected", item.applyOnSprite.itemID);
                    foreach (SyncName syncName in FindObjectsOfType<SyncName>())
                    {
                        syncName.UpdateColor();
                    }
                }
            }
            else
            {
                GameObject.Find("LoadingPlayer").GetComponent<ColourSetterLoad>().SetColor(item.foregroundColor);
                foreach (SyncName syncName in FindObjectsOfType<SyncName>())
                {
                    syncName.UpdateColor();
                }
            }
            if (GameObject.Find("LocalConnectionLobby"))
            {
                GameObject.Find("LocalConnectionLobby").GetComponent<PlayerManagement>().CmdUpdateColorAndName(SyncData.name, SyncData.color, PlayerPrefs.GetInt(ShopItemType.Skin.ToString() + "selected"));
            }
        }
    }
}
