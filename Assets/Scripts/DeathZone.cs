using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DeathZone : MonoBehaviour
{
    [FormerlySerializedAs("Manager")] public MainManager manager;

    private void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
        manager.GameOver();
    }
}
