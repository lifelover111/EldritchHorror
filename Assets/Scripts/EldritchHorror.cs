using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Phase
{
    Action, 
    Encounter, 
    Mythos
}




public class EldritchHorror : MonoBehaviour
{
    public static Dictionary<int, int[]> referenceDICT = new Dictionary<int, int[]>{ //playersCount => gates,clues,monsters
        { 1, new int[] { 1, 1, 1 } }, { 2, new int[] { 1, 1, 1 } }, 
        { 3, new int[] { 1, 2, 2 } }, { 4, new int[] { 1, 2, 2 } }, 
        { 5, new int[] { 2, 3, 2 } }, { 6, new int[] { 2, 3, 2 } }, 
        { 7, new int[] { 2, 4, 3 } }, { 8, new int[] { 2, 4, 3 } } };


    public static EldritchHorror eldritchHorror;
    static int playersCount = 2;
    public static Phase currentPhase = Phase.Action;
    public Player[] players;
    public Player currentPlayer;
    public List<Gate> activeGates = new List<Gate>();
    Player leadPlayer;
    [SerializeField] public GameObject investigatorPrefab;
    [SerializeField] public Sprite[] investigatorSprites;

    GameObject cluePrefab;
    GameObject trainTicketPrefab;
    GameObject shipTicketPrefab;
    GameObject eldritchTokenPrefab;
    Ancient ancient;
    ICup gates;
    ICup monsters;
    ICup epicMonsters;
    IDeck mysteriesDeck;
    IDeck researchEncDeck;
    IDeck specialEncDeck;
    IDeck expeditionEncDeck;
    IDeck americaEncDeck;
    IDeck europeEncDeck;
    IDeck asiaEncDeck;
    IDeck generalEncDeck;
    IDeck otherWorldEncDeck;
    IDeck mythosDeck;
    IDeck artifactsDeck;
    IDeck assetsDeck;
    IDeck conditionsDeck;
    IDeck spellsDeck;

    [SerializeField] public GameObject prefabEndTurn;


    public delegate void ReckoningDelegate();
    public event ReckoningDelegate ResolveReckoning;

    private void Awake()
    {
        eldritchHorror = this;

        ancient = new Ancient("Azathoth");

        List<string> test = new List<string>();
        test.Add("Astronomer");
        test.Add("Expedition Leader");

        //должно передаваться при создании пати
        players = new Player[playersCount];

        for (int i = 0; i < playersCount; i++)
        {
            GameObject go = Instantiate(investigatorPrefab);
            Player player = go.GetComponent<Player>();
            player.SetClass = test[i];
            players[i] = player;
            players[i].id = i;
        }

        leadPlayer = players[Random.Range(0, playersCount)];
    }

    private void Start()
    {
        PrepareGame(ancient, players);
    }


    private void PrepareGame(Ancient ancient, Player[] players)
    {
        currentPlayer = players[leadPlayer.id];

        ancient.prepare();

        GameBoard.gameBoard.PrepareGameBoard(ancient);

        mythosDeck = new MythosDeck(ancient, new MythosCup());

        CreateUI();

        currentPlayer.makesTurn = true;

    }

    public void NextPlayer()
    {
        if (currentPlayer.id == ((leadPlayer.id + playersCount - 1) % playersCount))
        {
            currentPhase++;
            if ((int)currentPhase == 3)
                currentPhase = Phase.Action;
        }
        if (currentPhase != Phase.Mythos)
        {
            UIWindow.UIwindow.DeleteWindow();
            currentPlayer.makesTurn = false;
            currentPlayer = players[(currentPlayer.id + 1) % playersCount];
            currentPlayer.makesTurn = true;
        }
        else
        {
            DoMythosPhase();
            NextPlayer();
        }
    }

    void DoMythosPhase()
    {
        print("Mythos phase");
        Mythos mythos = (Mythos)mythosDeck.TakeCard();
        switch(mythos.color)
        {
            case Mythos.MythosColor.green:
                AdvanceOmen();
                SpawnClues();
                break;
            case Mythos.MythosColor.yellow:
                AdvanceOmen();
                ResolveReckoning?.Invoke();
                SpawnGates();
                break;
            case Mythos.MythosColor.blue:
                SpawnClues();
                break;
        }
    }


    void CreateUI()
    {
        CreateEndTurn();
    }
    void CreateEndTurn()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject endTurn = Instantiate(prefabEndTurn);
        endTurn.transform.SetParent(canvas.transform, false);
    }



    void AdvanceOmen()
    {
        GameBoard.gameBoard.AdvanceOmen();
    }
    void SpawnClues()
    {
        for(int i = 0; i < referenceDICT[playersCount][1]; i++)
        {
            Location location = GameBoard.gameBoard.locations[Random.Range(0, GameBoard.gameBoard.locations.Length)];
            GameBoard.gameBoard.SpawnClue(location);
        }
    }
    void SpawnGates()
    {
        List<Location> locsWithGates = new List<Location>();
        foreach(Location location in GameBoard.gameBoard.locations)
        {
            if(location.canHaveGates && !location.haveGates)
                locsWithGates.Add(location);
        }
        for (int i = 0; i < referenceDICT[playersCount][0]; i++)
        {
            if (locsWithGates.Count == 0)
                break;
            int r = Random.Range(0, locsWithGates.Count);
            GameBoard.gameBoard.SpawnGate(locsWithGates[r]);
            locsWithGates.RemoveAt(r);
        }
    }
}
