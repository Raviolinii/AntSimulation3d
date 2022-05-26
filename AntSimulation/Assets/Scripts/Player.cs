using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed = 50f;
    public float borderMoveArea = 10f;
    private Vector2 moveLimitX = new Vector2(0f, 250f);
    private Vector2 moveLimitZ = new Vector2(-250f, 0f);

    public float scrollSpeed = 20f;
    public float minScroll = 50f;
    public float maxScroll = 120f;

    // Start is called before the first frame update
    void Start()
    {
        //layerAsLayerMask = (1 << layer);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 pos = transform.position;
        if (Input.GetButton("MoveRight") || Input.mousePosition.x >= Screen.width - borderMoveArea)
            pos.x += Time.deltaTime * moveSpeed;

        if (Input.GetButton("MoveLeft") || Input.mousePosition.x <= borderMoveArea)
            pos.x -= Time.deltaTime * moveSpeed;

        if (Input.GetButton("MoveUp") || Input.mousePosition.y >= Screen.height - borderMoveArea)
            pos.z += Time.deltaTime * moveSpeed;

        if (Input.GetButton("MoveDown") || Input.mousePosition.y <= borderMoveArea)
            pos.z -= Time.deltaTime * moveSpeed;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * Time.deltaTime * 100f;

        pos.x = Mathf.Clamp(pos.x, moveLimitX.x, moveLimitX.y);
        pos.y = Mathf.Clamp(pos.y, minScroll, maxScroll);
        pos.z = Mathf.Clamp(pos.z, moveLimitZ.x, moveLimitZ.y);
        transform.position = pos;
    }

}
