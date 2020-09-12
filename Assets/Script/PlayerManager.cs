using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    static readonly int WINNING_COUNT = 6;

    public GameObject PickUp;
    public GameObject PickUpGroup;

    void Start()
    {
        NetworkClient.connection.identity.GetComponent<PlayerManager>().CmdCreatePickUps();
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
}
