using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialMimic : MonoBehaviour
{
	[SerializeField] MeshRenderer sourceMeshRend;
	[SerializeField] List<MeshRenderer> destMeshRends = new List<MeshRenderer>();

	Material _mat;
	public Material Mat { get { return _mat; } }

	private void Awake()
	{
		if (sourceMeshRend == null)
			sourceMeshRend = GetComponent<MeshRenderer>();

		_mat = new Material(sourceMeshRend.material);

		if(destMeshRends.Count <= 0)
        {
			sourceMeshRend.material = _mat;
			return;
		}

		foreach(MeshRenderer meshRend in destMeshRends)
        {
			meshRend.material = _mat;
        }
	}

    private void OnDestroy()
    {
		Destroy(_mat);
    }
}
