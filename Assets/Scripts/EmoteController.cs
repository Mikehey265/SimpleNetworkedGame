using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class EmoteController : NetworkBehaviour
{
    [SerializeField] private Image emoteImage;
    [SerializeField] private Sprite emote1;
    [SerializeField] private Sprite emote2;
    [SerializeField] private float emoteLongevity;

    private bool showedEmote;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        emoteImage.enabled = false;
    }

    private void OnEnable()
    {
        PlayerController.OnEmoteInput += HandleEmoteInput;
    }

    private void OnDisable()
    {
        PlayerController.OnEmoteInput -= HandleEmoteInput;
    }

    private void HandleEmoteInput(int emoteType)
    {
        if(!IsOwner) return; 
        TriggerEmoteServerRpc(emoteType);
    }

    [ServerRpc]
    private void TriggerEmoteServerRpc(int emoteType)
    {
        ShowEmoteClientRpc(emoteType);
    }

    [ClientRpc]
    private void ShowEmoteClientRpc(int emoteType)
    {
        DisplayEmote(emoteType);
    }

    private void DisplayEmote(int emoteType)
    {
        if (emoteType == 1 && !showedEmote)
        {
            emoteImage.enabled = true;
            emoteImage.sprite = emote1;
            showedEmote = true;
            StartCoroutine(HideEmote(emoteLongevity));
        }
        else if (emoteType == 2 && !showedEmote)
        {
            emoteImage.enabled = true;
            emoteImage.sprite = emote2;
            showedEmote = true;
            StartCoroutine(HideEmote(emoteLongevity));
        }
    }

    private IEnumerator HideEmote(float delay)
    {
        yield return new WaitForSeconds(delay);
        emoteImage.enabled = false;
        showedEmote = false;
    }
}
