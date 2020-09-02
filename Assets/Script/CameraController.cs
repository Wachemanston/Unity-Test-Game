using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraController : NetworkBehaviour
{
    public PlayerController player;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 10, -10);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        player = NetworkClient.connection.identity.GetComponent<PlayerController>();
        transform.position = offset + player.transform.position;
    }
}
