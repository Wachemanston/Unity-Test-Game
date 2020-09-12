using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    static readonly int WINNING_COUNT = 6;

    public float speed = 10.0f;
    public TextMeshProUGUI countText;
    public GameObject winTextObj;

    public GameObject PickUp;
    public GameObject PickUpGroup;

    private int count;
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        SetCountText();
        winTextObj.SetActive(false);

        NetworkClient.connection.identity.GetComponent<PlayerController>().CmdCreatePickUps();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb = NetworkClient.connection.identity.GetComponent<Rigidbody>();
        rb.AddForce(movement * speed);
        RpcUpdatePlayersPos(NetworkClient.connection.identity.netId, movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);

            count++;
            SetCountText();
        }
    }

    private void SetCountText()
    {
        countText.text = "Count : " + NetworkClient.connection.identity.GetComponent<PlayerController>().count.ToString();
        if (count >= WINNING_COUNT)
        {
            winTextObj.SetActive(true);
        }
    }

    Vector3 getRandomPos()
    {
        int range = 10;
        return new Vector3(Random.Range(-range, range), 1, Random.Range(-range, range));
    }
    
    [Command]
    public void CmdCreatePickUps()
    {
        for (int i = 0; i < WINNING_COUNT + 2; i++)
        {
            GameObject pickUp = Instantiate(PickUp, getRandomPos(), Quaternion.identity);
            NetworkServer.Spawn(pickUp, connectionToClient);
            RpcShowPickUps(pickUp);
        }
    }
    
    [ClientRpc]
    public void RpcShowPickUps(GameObject pickUp)
    {
        PickUpGroup = GameObject.Find("PickUpGroups");
        pickUp.GetComponent<Transform>().SetParent(PickUpGroup.transform, false);
    }

    [ClientRpc] // TODO: Update value from client to server
    public void RpcUpdatePlayersPos(uint uid, Vector3 force)
    {
        if (NetworkIdentity.spawned.TryGetValue(uid, out NetworkIdentity identity))
        {
            identity.gameObject.GetComponent<Rigidbody>().AddForce(force);
        }
    }
}
