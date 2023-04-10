using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEndTurn : MonoBehaviour
{    
    public void EndTurn()
    {
        EldritchHorror.eldritchHorror.NextPlayer();
    }
}
