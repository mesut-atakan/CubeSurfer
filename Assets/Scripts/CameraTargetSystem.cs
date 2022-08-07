using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetSystem : MonoBehaviour
{
    [Header("Target features")]
    [Tooltip("object to track")]
    public GameObject target;

    [Tooltip("tracking speed")]
    public float targetSpeed;

    [Range(-8, 3)] public float Yaxsis;

    private Vector3 distance;


    private void Start() {
        distance = target.transform.position + this.transform.position;
        var pos = distance;
        pos.y += Yaxsis;
        distance = pos;
    }

    private void FixedUpdate() {
        targetSystem();
    }


    private void targetSystem()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + distance, targetSpeed * Time.fixedDeltaTime);
    }
}
