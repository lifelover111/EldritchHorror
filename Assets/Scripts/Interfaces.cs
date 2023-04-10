using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{

}

public interface IDeck
{
    public Card TakeCard();
    public void Shuffle();
}


public interface ICup
{

}