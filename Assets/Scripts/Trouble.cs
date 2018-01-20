using System;
using UnityEngine;

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

    // Deal with it !
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
