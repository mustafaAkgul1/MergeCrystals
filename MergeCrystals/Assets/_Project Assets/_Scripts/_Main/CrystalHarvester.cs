using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalHarvester : MonoBehaviour
{
    Vector3 dockPosition = new Vector3(-394.56f, -672f, 864f);
    bool isDragging = false;
    public RectTransform recTransform;
    Vector3 lastPos;
    public GameObject dockObj;

    void Start()
    {
        lastPos = new Vector3(recTransform.position.x, recTransform.position.y, recTransform.position.z);
    }

    private void OnMouseDown()
    {
        isDragging = true;
        GameManager._instance.heldCrystalHarvester = gameObject;

    } // OnMouseDown()

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }

    } // OnMouseDrag()

    private void OnMouseUp()
    {
        isDragging = false;
        GameManager._instance.heldCrystalHarvester = null;

        GetComponent<RectTransform>().position = dockObj.GetComponent<RectTransform>().position;

    } // OnMouseUp()

} // class
