using UnityEngine;
using DG.Tweening;

// This script is responsible for following the player in game.
public class CameraMovement : MonoBehaviour
{
    private GameObject playerObject;
    [SerializeField] private Transform _playerTransform;
    private Transform _camera;

    private Vector3 _offset;

    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = playerObject.transform;

        _camera = this.transform;

        _offset = _camera.position - _playerTransform.position;
    }

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        var targetPos = new Vector3(_playerTransform.position.x + _offset.x, _camera.position.y, _playerTransform.position.z + _offset.z);
        _camera.position = targetPos;
    }
}