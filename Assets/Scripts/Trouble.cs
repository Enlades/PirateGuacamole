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

    public Equipment.EquipmentType Requisite;

    public Trouble(int p_Level) {
        Progress = 0;

        Level = p_Level;
    }

    // Dew it !
    public void Deal(Action p_CallBack, Equipment.EquipmentType p_EquipmentType) {
        if (Requisite != Equipment.EquipmentType.Nill) {
            if (p_EquipmentType != Requisite) {
                Debug.Log("Fucked : " + p_EquipmentType);
                return;
            }
        }
            

        Progress += Time.deltaTime / Level * 10;

        if (Progress >= 10) {
            if (FinishEvent != null) {
                p_CallBack.Invoke();
                FinishEvent.Invoke();
            }
        }
    }

    public static Trouble GetReloadTrouble() {
        Trouble t = new Trouble(1);
        t.Requisite = Equipment.EquipmentType.CannonBall;

        return t;
    }

    public static Trouble GetFireTrouble() {
        Trouble t = new Trouble(1);
        t.Requisite = Equipment.EquipmentType.FireStick;

        return t;
    }
}
