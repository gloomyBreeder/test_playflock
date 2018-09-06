using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserBehaviour : MonoBehaviour, IPointerClickHandler
{
    private readonly float _distance = 1f;
    private Ray _ray;
    private const string PATH_TO_RECT = "Prefabs/rectangle";
    // detect if rectangle was clicked for the first time in order to create a link
    private bool _firstTouch = true;
    private LineRenderer _newLine;
    private Vector2 _firstClick;

    private void SpawnObject()
    {
        var obj = Instantiate(Resources.Load(PATH_TO_RECT),
            _ray.origin + (_ray.direction * _distance),
            Quaternion.Euler(0, 0, 0)) as GameObject;
        
        if (obj == null)
        {
            Debug.LogError("oops something goes wrong");
            return;
        }

        //add component for rectangle behaviour
        obj.AddComponent<RectangleBehaviour>();
        
        // it is possible to create some sort of array of colors and randomly choose colors from it but it is less random
        // so i make it this way
        Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        obj.GetComponent<Renderer>().material.color = newColor;
    }

    private void CreateLink(GameObject rect)
    {
        // commented because it works not as expected
        /*
        if (_firstTouch)
        {
            if (rect.GetComponent<LineRenderer>() == null)
                _newLine = rect.AddComponent<LineRenderer>();
            _newLine.useWorldSpace = true;
            _firstClick = rect.transform.localPosition;
            _firstTouch = false;
        }
        else
        {
            if (_newLine == null)
                return;
            Vector2 secondClick = rect.transform.localPosition;
            _newLine.startWidth = 0.3f;
            _newLine.endWidth = 0.3f;
            _newLine.SetPosition(0, new Vector3(_firstClick.x, _firstClick.y, 0));
            _newLine.SetPosition(1, new Vector3(secondClick.x, secondClick.y, 0));
            _firstTouch = true;
        }*/

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int i = eventData.clickCount;
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 point2d = new Vector2(point.x, point.y);
        RaycastHit2D raycast2d = Physics2D.Raycast(point2d, point2d);

        if (raycast2d.collider == null && i != 2)
            SpawnObject();
        else
        {
            // check how many clicks user did
            if (i == 1 && raycast2d.collider != null)
                CreateLink(raycast2d.collider.gameObject);
            else
            {
                if (raycast2d.collider != null)
                    Destroy(raycast2d.collider.gameObject);
            }
        }
    }
    
}
