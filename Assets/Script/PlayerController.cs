using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    static readonly int WINNING_COUNT = 6;

    public float speed = 10.0f;
    public TextMeshProUGUI countText;
    public GameObject winTextObj;

    private int count;
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        count = 0;
        SetCountText();
        winTextObj.SetActive(false);
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
        rb.AddForce(movement * speed);
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
        countText.text = "Count : " + count.ToString();
        if (count >= WINNING_COUNT)
        {
            winTextObj.SetActive(true);
        }
    }
}
