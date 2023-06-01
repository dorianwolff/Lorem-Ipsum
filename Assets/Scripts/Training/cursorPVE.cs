using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorPVE : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    private void Update()
    {
        Vector3 pos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 2);//maybe z = 0
    }
}
