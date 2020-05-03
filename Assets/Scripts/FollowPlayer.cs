using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    private Vector3 _offset = new Vector3(0, 8, -5);

    // Late Update used for a follow camera because it tracks objects that might have moved inside Update
    void LateUpdate()
    {
        // Offset the camera behind the player by adding to the player's position
        transform.position = _player.transform.position + _offset;
    }
}
