using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Gun : MonoBehaviour
{
    private Tower _tower;
    private Transform _transformSpawnPoint;
    public Transform TransformSpawnPoint { get { return _transformSpawnPoint; } }
    private void Start()
    {
        _tower = GetComponentInParent<Tower>();
        _transformSpawnPoint = GetComponentInChildren<Transform>();
    }
    private void Update()
    {
        RotateGun();
    }

    private void RotateGun()
    {
        if(_tower.ClosedEnemy != null)
        {
            Vector3 relativePos = _tower.ClosedEnemy.position - transform.position;
            relativePos = new Vector3(relativePos.x, relativePos.y - 120, relativePos.z);
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            rotation.x = 0; // Запретить вращение вокруг оси X
            rotation.z = 0; // Запретить вращение вокруг оси Z
            transform.rotation = rotation;
        }
    }
}
