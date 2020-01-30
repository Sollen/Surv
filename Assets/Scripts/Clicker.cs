using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clicker: MonoBehaviour
{
    private void OnMouseDown()
    {
        var player = GameObject.FindGameObjectsWithTag("Darwin");
        player.FirstOrDefault()?.GetComponent<PlayerController>().ChangeTarget(transform.position);
    }
}