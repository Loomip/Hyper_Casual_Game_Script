using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPosRotate : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Update()
    {
        transform.rotation = player.rotation;
    }
}
