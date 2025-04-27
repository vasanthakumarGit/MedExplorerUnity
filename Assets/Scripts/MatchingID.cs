using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to all the sprites in right side.
[RequireComponent(typeof(BoxCollider2D))]

public class MatchingID : MonoBehaviour
{
    [SerializeField] private int matchId;

    public int Get_ID()
    {
        return matchId;
    }
}
