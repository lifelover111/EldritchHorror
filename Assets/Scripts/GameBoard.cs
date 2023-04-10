using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Omen 
{
    green,
    blue,
    red
}

public class GameBoard : MonoBehaviour
{
    [SerializeField] public GameObject omenTokenPrefab;
    [SerializeField] public GameObject doomTokenPrefab;
    [SerializeField] public GameObject expeditionTokenPrefab;
    [SerializeField] public GameObject mysteryTokenPrefab;
    [SerializeField] public GameObject rumorTokenPrefab;
    [SerializeField] public GameObject clueTokenPrefab;
    [SerializeField] public GameObject eldritchTokenPrefab;
    [SerializeField] public GameObject gateTokenPrefab;
    [SerializeField] public GameObject[] placesDoom;
    [SerializeField] public GameObject[] placesOmen;

    public Location[] locations;

    private GameObject doomToken;
    private GameObject omenToken;

    public static GameBoard gameBoard;
    private int currentDoom;
    public Omen currentOmen = Omen.green;
    private Asset[] reserve = new Asset[4];
    private static Dictionary<int, Omen> omenDICT = new Dictionary<int, Omen>{ {0, Omen.green }, {1, Omen.blue }, {2, Omen.red }, {3, Omen.blue } };
    private int omenCounter = 0;

    private void Awake()
    {
        gameBoard = this;
        placesOmen[0].GetComponent<placeOmen>().omen = Omen.green;
        placesOmen[1].GetComponent<placeOmen>().omen = Omen.blue;
        placesOmen[2].GetComponent<placeOmen>().omen = Omen.red;
        placesOmen[3].GetComponent<placeOmen>().omen = Omen.blue;
        locations = gameObject.GetComponentsInChildren<Location>();
    }

    public void PrepareGameBoard(Ancient ancient)
    {
        doomToken = Instantiate(doomTokenPrefab);
        omenToken = Instantiate(omenTokenPrefab);
        print(ancient.name);
        SetDoom(ancient.doomOnStart);
        currentOmen = omenDICT[0];
        omenToken.transform.position = placesOmen[0].transform.position;
        ChooseExpedition();
    }
    void ChooseExpedition()
    {
        List<Location> expeditionalLocations = new List<Location>();
        foreach (Location location in locations)
        {
            location.isExpedition = false;
            if (location.canBeExpeditional)
                expeditionalLocations.Add(location);
        }
        int r = Random.Range(0, expeditionalLocations.Count);
        expeditionalLocations[r].isExpedition = true;
        GameObject go = Instantiate(expeditionTokenPrefab);
        go.transform.position = expeditionalLocations[r].transform.position;
    }
    public void SetDoom(int doom)
    {
        currentDoom = doom;
        doomToken.transform.position = placesDoom[currentDoom].transform.position;
    }

    public void AdvanceOmen()
    {
        omenCounter++;
        omenCounter %= 4;
        omenToken.transform.position = placesOmen[omenCounter].transform.position;
        currentOmen = omenDICT[omenCounter];
        foreach(Gate g in EldritchHorror.eldritchHorror.activeGates)
        {
            if (g.omen == currentOmen)
                SetDoom(currentDoom - 1);
        }
    }

    public void SpawnClue(Location loc)
    {
        GameObject go = Instantiate(clueTokenPrefab);
        go.transform.position = loc.gameObject.transform.position;
        go.transform.SetParent(loc.gameObject.transform, true);
    }

    public void SpawnGate(Location loc)
    {
        loc.haveGates = true;
        GameObject go = Instantiate(gateTokenPrefab);
        go.transform.position = loc.gameObject.transform.position;
        go.transform.SetParent(loc.gameObject.transform, true);
        int temp = Random.Range(0, 3);
        Gate gate = go.GetComponent<Gate>();
        gate.omen = (Omen)temp;
        switch (temp)
        {
            case 0:
                go.GetComponent<SpriteRenderer>().color = new Color(0,0.35f,0,0.95f);
                break;
            case 1:
                go.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0.35f, 0.95f);
                break;
            case 2:
                go.GetComponent<SpriteRenderer>().color = new Color(0.35f, 0, 0, 0.95f);
                break;
        }
        EldritchHorror.eldritchHorror.activeGates.Add(gate);
    }
}
