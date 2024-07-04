using UnityEngine;
using DG.Tweening;

// This script is responsible for following the player in game.
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private Transform _camera;

    private Vector3 _offset;

    private void Awake()
    {
        _camera = this.transform;

        _offset = _camera.position - _player.position;
    }

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        var targetPos = new Vector3(_player.position.x + _offset.x, _camera.position.y, _player.position.z + _offset.z);
        _camera.position = targetPos;
    }
}