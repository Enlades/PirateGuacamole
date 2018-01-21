using System;
using UnityEngine;

public class PumpController : MonoBehaviour, IUsable
{
    public Trouble Use(Action p_CallBack, CharacterController p_User)
    {
        GameManager.WaterLevel -= Time.deltaTime * 0.03f;
        return null;
    }
}