using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mythos : Card
{
    public enum MythosColor
    {
        green,
        yellow,
        blue
    }
    public MythosColor color;

    public Mythos() { }
}

public class MythosDeck : IDeck
{
    List<Mythos> deck;

    public MythosDeck(Ancient ancient, MythosCup cup)
    {
        deck = new List<Mythos>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < ancient.mythosChapter1[i]; j++)
            {
                int r = Random.Range(0, cup.mythos[i].Count);
                deck.Add(cup.mythos[i][r]);
                cup.mythos[i].RemoveAt(r);
            }
        }
        ShuffleAt(0, ancient.mythosChapter1[0] + ancient.mythosChapter1[1] + ancient.mythosChapter1[2]);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < ancient.mythosChapter2[i]; j++)
            {
                int r = Random.Range(0, cup.mythos[i].Count);
                deck.Add(cup.mythos[i][r]);
                cup.mythos[i].RemoveAt(r);
            }
        }
        ShuffleAt(ancient.mythosChapter1[0] + ancient.mythosChapter1[1] + ancient.mythosChapter1[2], ancient.mythosChapter2[0] + ancient.mythosChapter2[1] + ancient.mythosChapter2[2]);


        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < ancient.mythosChapter3[i]; j++)
            {
                int r = Random.Range(0, cup.mythos[i].Count);
                deck.Add(cup.mythos[i][r]);
                cup.mythos[i].RemoveAt(r);
            }
        }
        ShuffleAt(ancient.mythosChapter2[0] + ancient.mythosChapter2[1] + ancient.mythosChapter2[2], ancient.mythosChapter3[0] + ancient.mythosChapter3[1] + ancient.mythosChapter3[2]);
    }

    public Card TakeCard()
    {
        Mythos card = deck[0];
        deck.RemoveAt(0);
        return card;
    }
    public void Shuffle() { }
    private void ShuffleAt(int fromInclusive, int toExclusive)
    {
        List<Mythos> tmpDeck = new List<Mythos>();
        for (int i = fromInclusive; i < toExclusive; i++)
        {
            tmpDeck.Add(deck[i]);
        }
        for (int i = fromInclusive; i < toExclusive; i++)
        {
            int r = Random.Range(0, tmpDeck.Count);
            deck[i] = tmpDeck[r];
            tmpDeck.RemoveAt(r);
        }
    }
}


public class MythosCup : ICup
{
    public List<Mythos>[] mythos;
    public MythosCup()
    {
        //Прикрутить сериализацию!!!
        mythos = new List<Mythos>[3];
        mythos[0] = new List<Mythos>();
        mythos[1] = new List<Mythos>();
        mythos[2] = new List<Mythos>();
        for(int i = 0; i < 15; i++)
        {
            mythos[0].Add(new Mythos());
            mythos[0][i].color = Mythos.MythosColor.green;
            mythos[1].Add(new Mythos());
            mythos[1][i].color = Mythos.MythosColor.yellow;
            mythos[2].Add(new Mythos());
            mythos[2][i].color = Mythos.MythosColor.blue;
        }
    }
}