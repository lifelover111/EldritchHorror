using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Action
{
    public string name;
    public delegate void ActionDelegate();
    public ActionDelegate action;
    public Action(ActionDelegate action, string name)
    {
        this.name = name;
        this.action = action;
    }
}

public class Investigator
{
    public string Class;
    public int health;
    public int sanity;
    public int lore;
    public int influence;
    public int observation;
    public int strength;
    public int will;
    public string[] assets;
    public string[] spells;
    public int cluesCount;
    public int trainTickets;
    public int shipTickets;
    public int skillImprovements;
    public string startLocation;
}

public class Player : MonoBehaviour
{
    public int id;
    private string invClass;
    private string Class;
    private Investigator _investigator;
    private int numActions = 2;
    private int _maxHealth;
    private int _maxSanity;
    private int _currentHealth;
    private int _currentSanity;
    private int _lore;
    private int _influence;
    private int _observation;
    private int _strength;
    private int _will;
    public int trainTickets;
    public int shipTickets;
    public Asset[] assets;
    public Spell[] spells;
    public Condition[] conditions;
    public int cluesCount;
    private bool _makesTurn;

    Action.ActionDelegate action;
    public Location currentLocation;
    public Location contactedLocation;
    private GameObject investigatorToken;

    List<Action> possibleActions;


    public Investigator investigator { get { return _investigator; } private set { _investigator = value; } }
    public int maxHealth { get { return _maxHealth; } private set { _maxHealth = value; } }
    public int maxSanity { get { return _maxSanity; } private set { _maxSanity = value; } }
    public int currentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public int currentSanity { get { return _currentSanity; } set { _currentSanity = value; } }
    public int lore { get { return _lore; } private set { _lore = value; } }
    public int influence { get { return _influence; } private set { _influence = value; } }
    public int observation { get { return _observation; } private set { _observation = value; } }
    public int strength { get { return _strength; } private set { _strength = value; } }
    public int will { get { return _will; } private set { _will = value; } }
    public bool makesTurn { get { return _makesTurn; } set { _makesTurn = value; } }
    public string SetClass { set { Class = value; } }
    

    private void Start()
    {
        investigator = XML_Reader.DeserializeInvestigator(Class);
        invClass = investigator.Class;
        _maxHealth = investigator.health;
        _maxSanity = investigator.sanity;
        currentHealth = _maxHealth;
        currentSanity = _maxSanity;
        lore = investigator.lore;
        influence = investigator.influence;
        observation = investigator.observation;
        strength = investigator.strength;
        will = investigator.will;
        cluesCount = investigator.cluesCount;
        trainTickets = investigator.trainTickets;
        shipTickets = investigator.shipTickets;
        foreach (Location loc in GameBoard.gameBoard.locations)
            if (loc.name == investigator.startLocation)
                currentLocation = loc;

        foreach (Sprite s in EldritchHorror.eldritchHorror.investigatorSprites)
            if (s.name == invClass)
                gameObject.GetComponent<SpriteRenderer>().sprite = s;
        gameObject.transform.position = currentLocation.gameObject.transform.position;
        GetBaseActions();
    }
    private void Update()
    {
        if (makesTurn)
            MakeTurn(EldritchHorror.currentPhase);
        TurnControl();
    }


    private void GetBaseActions()
    {
        possibleActions = new List<Action>();

        possibleActions.Add(new Action(() =>
        {
            if (currentHealth < maxHealth)
                currentHealth++;
            if (currentSanity < maxSanity)
                currentSanity++;
            EldritchHorror.print("Rest");
            numActions--;
        }, "Rest"));


        possibleActions.Add(new Action(() =>
       {
           UIWindow.UIwindow.HideWindow();
           foreach (Paths p in currentLocation.AvailablePaths)
           {
               p.Location.GetComponent<SpriteRenderer>().enabled = true;
               p.Location.GetComponent<SphereCollider>().enabled = true;
           }

           action = new Action.ActionDelegate(() =>
           {
               currentLocation = contactedLocation;
               gameObject.transform.position = currentLocation.transform.position;
               if (CanContinueTravel())
               {
                   UIController.CreateSelectionBox(new Selection(() =>
                   {
                       ContinueTravel();
                   }, "Continue travel"), new Selection(() => { UIWindow.UIwindow.ShowWindow(); }, "Stop travel"));
               }
               else
               {
                   UIWindow.UIwindow.ShowWindow();
               }
           });

           EldritchHorror.print("Travel");
           numActions--;
       }, "Travel"));

        possibleActions.Add(new Action(() =>
       {
           if (currentLocation.locType == LocType.City)
           {
               UIWindow.UIwindow.HideWindow();
               UIController.CreateSelectionBox(new Selection(() => { trainTickets++; UIWindow.UIwindow.ShowWindow(); }, "Train ticket"), new Selection(() => { shipTickets++; UIWindow.UIwindow.ShowWindow(); }, "Ship ticket"));
               numActions--;
           }
       }, "Prepare for travel"));
    }

    
    public void ContinueAction()
    {
        action.Invoke();
        EndAction();
    }

    private void EndAction()
    {
        if (action != null)
            action = null;
        foreach (Location loc in GameBoard.gameBoard.locations)
        {
            loc.GetComponent<SpriteRenderer>().enabled=false;
            loc.GetComponent<SphereCollider>().enabled=false;
        }
    }

    public void MakeTurn(Phase phase)
    {
        _makesTurn = false;
        EldritchHorror.print(invClass + " makes a turn in location " + currentLocation.name +" current phase: " + EldritchHorror.currentPhase);
        switch (phase)
        {
            case Phase.Action:
                DoActionPhase();
                break;
            case Phase.Encounter:
                DoEncounterPhase();
                break;
            default:
                break;
        }
        
    }

    private void DoActionPhase()
    {
        UIWindow.UIwindow.CreateWindow(possibleActions.ToArray());
    }

    private void DoEncounterPhase()
    {
        List<Selection> availableEncounters = new List<Selection>();
        availableEncounters.Add(new Selection(() => { }, "Common"));
        if (currentLocation.isExpedition)
            availableEncounters.Add(new Selection(() => { }, "Expeditional"));
        if (currentLocation.haveGates)
            availableEncounters.Add(new Selection(() => { }, "Outer worlds"));
        foreach (Transform child in currentLocation.transform)
        {
            if (child.gameObject.CompareTag("Clue"))
            {
                availableEncounters.Add(new Selection(() => { }, "Search"));
                break;
            }
        }
        if (currentLocation.additionalTag != "")
            availableEncounters.Add(new Selection(() => { }, currentLocation.additionalTag));
        UIController.CreateSelectionBox(availableEncounters);
    }

    void TurnControl()
    {
        if(numActions <= 0)
        {
            numActions = 2;
            UIWindow.UIwindow.DeleteWindow();
        }
    }

    bool CanContinueTravel()
    {
        if(trainTickets > 0)
        {
            foreach(Paths p in currentLocation.AvailablePaths)
            {
                if(p.Type == PathType.Train)
                    return true;
            }
        }
        if (shipTickets > 0)
        {
            foreach (Paths p in currentLocation.AvailablePaths)
            {
                if (p.Type == PathType.Ship)
                    return true;
            }
        }
        return false;
    }

    void ContinueTravel()
    {
        UIWindow.UIwindow.HideWindow();
        if (trainTickets > 0)
            foreach (Paths p in currentLocation.AvailablePaths)
            {
                if (p.Type == PathType.Train)
                {
                    p.Location.GetComponent<SpriteRenderer>().enabled = true;
                    p.Location.GetComponent<SphereCollider>().enabled = true;
                }
            }

        if (shipTickets > 0)
            foreach (Paths p in currentLocation.AvailablePaths)
            {
                if (p.Type == PathType.Ship)
                {
                    p.Location.GetComponent<SpriteRenderer>().enabled = true;
                    p.Location.GetComponent<SphereCollider>().enabled = true;
                }
            }

        action = new Action.ActionDelegate(() =>
        {
            foreach (Paths p in currentLocation.AvailablePaths)
            {
                if (p.Location == contactedLocation.gameObject)
                {
                    if (p.Type == PathType.Train)
                        trainTickets--;
                    if (p.Type == PathType.Ship)
                        shipTickets--;
                }
            }
            currentLocation = contactedLocation;
            gameObject.transform.position = currentLocation.transform.position;
            if (CanContinueTravel())
            {
                UIController.CreateSelectionBox(new Selection(() => { ContinueTravel(); }, "Continue travel"), new Selection(() => { UIWindow.UIwindow.ShowWindow(); }, "Stop travel"));
            }
            else
            {
                UIWindow.UIwindow.ShowWindow();
            }
        });
    }
}
