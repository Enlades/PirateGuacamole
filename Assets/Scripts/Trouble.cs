using System;
using UnityEngine;

[Serializable]
public class Trouble {
    public float Progress;
    public int Level;

    public Action FinishEvent;

    public Trouble(int p_Level) {
        Progress = 0;

        Level = p_Level;
    }

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
