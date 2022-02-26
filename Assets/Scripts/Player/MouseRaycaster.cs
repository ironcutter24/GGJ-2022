using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;

public class MouseRaycaster : Singleton<MouseRaycaster>
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject cursor3D;

    private RaycastHit hit;
    public static RaycastHit Hit { get { return _instance.hit; } }

    private bool error = true;
    public static bool Error { get { return _instance.error; } }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        error = !Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
    }

    private void LateUpdate()
    {
        if (!error && cursor3D != null)
            cursor3D.transform.position = hit.point;
    }
}
