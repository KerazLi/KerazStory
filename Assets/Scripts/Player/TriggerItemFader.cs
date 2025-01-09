using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemFader[] faders = other.GetComponentsInChildren<ItemFader>();
        foreach (ItemFader fader in faders)
        {
            fader.FadeOut();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        ItemFader[] faders = other.GetComponentsInChildren<ItemFader>();
        foreach (ItemFader fader in faders)
        {
            fader.FadeIn();
        }
    }
}
