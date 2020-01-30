﻿using UnityEngine;

namespace Services
{
    public class FollowCamera : MonoBehaviour
    {
        public Transform target;
        public float smooth= 5.0f;
        public Vector3 offset = new Vector3(0, 2, -5);
        void  Update ()
        {
            transform.position = Vector3.Lerp (transform.position, target.position + offset, Time.deltaTime * smooth);
        } 


    }
}
