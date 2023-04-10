using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AncientMethods
{
    public delegate void prepareDelegate();
    public delegate void cultistDelegate();
    public delegate void ancientEffectsDelegate();
    public delegate void onAwakeningDelegate();

    public static prepareDelegate GetPrepareDelegate(string name)
    {
        prepareDelegate tempDelegate = null;
        switch (name)
        {
            case "Azathoth":
                tempDelegate = azathotPrepare;
                break;
            case "Yogg-Sothoth":
                tempDelegate = yoggsothothPrepare;
                break;
            case "Cthulhu":
                tempDelegate = cthulhuPrepare;
                break;
            default:
                break;
        }
        return tempDelegate;
    }

    public static void azathotPrepare()
    {
        GameObject token = GameBoard.Instantiate(GameBoard.gameBoard.eldritchTokenPrefab);
        token.transform.SetParent(GameBoard.gameBoard.transform, true);
        token.transform.position = GameBoard.gameBoard.placesOmen[0].transform.position;
        foreach (GameObject go in GameBoard.gameBoard.placesOmen)
        {
            if (go.GetComponent<placeOmen>().omen == Omen.green)
                go.GetComponent<placeOmen>().eldritchTokens++;
        }
    }

    public static void yoggsothothPrepare()
    {
        PrepareSpecialEncounters("Yogg-Sothoth");
    }
    public static void cthulhuPrepare()
    {
        PrepareSpecialEncounters("Cthulhu");
    }

    public static void PrepareSpecialEncounters(string name)
    {
        //EldritchHorror.eldritchHorror.
    }
}
