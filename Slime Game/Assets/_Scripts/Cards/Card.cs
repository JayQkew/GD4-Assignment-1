using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Card
{
    public string cardName;
    public Modifier[] modifiers;
    public GameObject[] abilities;
    public string description;
    public Sprite icon;
    
    // the abilities will subscribe events to the input handler or where necessary when spawned
}
