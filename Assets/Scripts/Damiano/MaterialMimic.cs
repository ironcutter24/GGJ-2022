using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialMimic : MonoBehaviour
{
	Material mat;

	private void Awake()
	{
		var mesh = GetComponent<MeshRenderer>();
		mat = new Material(mesh.material);
		mesh.material = mat;
	}

    private void OnDestroy()
    {
		Destroy(mat);
    }
}
