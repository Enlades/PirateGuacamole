using System;
using UnityEngine;

// This class is used for minigames that are generated for player
// The progress of the minigame, how hard it is, some shit more in future !!
[Serializable]
public class Trouble {
    // Progress value
    public float Progress;
    // More time it takes, the higher level is.
    public int Level;
    // I came early.
    public Action FinishEvent;

    public Trouble(int p_Level) {
        Progress = 0;

        Level = p_Level;
    }

    // Dew it !
    public void Deal(Action p_CallBack) {
        Progress += Time.deltaTime / Level * 10;

        if (Progress >= 10) {
            if (FinishEvent != null) {
                p_CallBack.Invoke();
                FinishEvent.Invoke();
            }
        }
    }
}
