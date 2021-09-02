using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IPunInstantiateMagicCallback
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private InputReader _inputReader;
    
    private Camera _camera;
    private bool _isMoving;

    private void Awake()
    {
        _camera = Camera.main;
        _inputReader.clickToMoveEvent += Move;
        _inputReader.clickToMoveCanceledEvent += CancelMove;
        
        if(GetComponent<PhotonView>().IsMine)
            return;

        GetComponent<Character>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
    }

    private void OnDestroy()
    {
        _inputReader.clickToMoveEvent -= Move;
        _inputReader.clickToMoveCanceledEvent -= CancelMove;
    }

    private void Move()
    {
        _isMoving = true;
    }

    private void CancelMove()
    {
        _isMoving = false;
    }

    public void FixedUpdate()
    {
        if (_isMoving)
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = _camera.ScreenPointToRay(_inputReader.pointerPosition);

            if (plane.Raycast(ray, out var point))
            {
                _navMeshAgent.SetDestination(ray.GetPoint(point));
            }

            _isMoving = false;
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = gameObject;
    }
}