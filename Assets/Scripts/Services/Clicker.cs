using System.Linq;
using Controllers;
using UnityEngine;

namespace Services
{
    public class Clicker: MonoBehaviour
    {
        private void OnMouseDown()
        {
            var player = GameObject.FindGameObjectsWithTag("Darwin");
            Debug.Log("Click");
            player.FirstOrDefault()?.GetComponent<PlayerController>().ChangeTarget(transform.position);
        }
    }
}