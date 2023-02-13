using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using BeatsBang.Core.Singleton;

public class ScoreManager : Singleton<ScoreManager>
{
    public int playerAKill = 0;
    public int playerADeath = 0;
    public int playerBKill = 0;
    public int playerBDeath = 0;
}
