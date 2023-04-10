using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PathType
{
    Train, Ship, Uncharted
}

public enum LocType
{
    City, Sea, Wilderness
}

[System.Serializable]
public struct Paths
{
    public GameObject Location;
    public PathType Type;
}

public class Location : MonoBehaviour
{
    [SerializeField] public string locationName;
    [SerializeField] public Paths[] AvailablePaths;
    [SerializeField] public LocType locType;
    [SerializeField] public bool canBeExpeditional = false;
    [SerializeField] public bool canHaveGates = false;
    [SerializeField] public string additionalTag;

    [System.NonSerialized]
    public bool isExpedition = false;
    [System.NonSerialized]
    public bool haveGates = false;

    private void Update()
    {
        
    }

    private void OnMouseDown()
    {
        EldritchHorror.eldritchHorror.currentPlayer.contactedLocation = this;
        EldritchHorror.eldritchHorror.currentPlayer.ContinueAction();
    }
}
