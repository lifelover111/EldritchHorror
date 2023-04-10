using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ancient
{
    public int id;
    public string name;
    public int doomOnStart;

    //green/yellow/blue:
    public int[] mythosChapter1 = new int[3];
    public int[] mythosChapter2 = new int[3];
    public int[] mythosChapter3 = new int[3];
    public int mysteriesCountToWin;

    private AncientMethods.prepareDelegate _prepare;
    private AncientMethods.cultistDelegate _cultist;
    private AncientMethods.ancientEffectsDelegate[] _ancientEffects;
    private AncientMethods.onAwakeningDelegate[] _onAwakening;


    [System.Xml.Serialization.XmlIgnore] public AncientMethods.prepareDelegate prepare { get { return _prepare; } }
    [System.Xml.Serialization.XmlIgnore] public AncientMethods.cultistDelegate cultist { get { return _cultist; } }
    [System.Xml.Serialization.XmlIgnore] public AncientMethods.ancientEffectsDelegate[] ancientEffects { get { return _ancientEffects; } }
    [System.Xml.Serialization.XmlIgnore] public AncientMethods.onAwakeningDelegate[] onAwakening { get { return _onAwakening; } }

    public Ancient() { }
    public Ancient(string name)
    {
        Ancient temp = XML_Reader.DeserializeAncient(name);
        id = temp.id;
        this.name = temp.name;
        doomOnStart = temp.doomOnStart;
        mythosChapter1 = temp.mythosChapter1;
        mythosChapter2 = temp.mythosChapter2;
        mythosChapter3 = temp.mythosChapter3;
        mysteriesCountToWin = temp.mysteriesCountToWin;
        _prepare = AncientMethods.GetPrepareDelegate(this.name);
    }
}
