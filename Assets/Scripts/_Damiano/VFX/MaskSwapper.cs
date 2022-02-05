using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskSwapper : MonoBehaviour
{
	[SerializeField] SkinnedMeshRenderer playerDress;

	[SerializeField] MeshRenderer maskHunter;
	[SerializeField] MeshRenderer maskPrey;
	
	private void Awake()
	{
		PlayerState.OnStateTransition += SetMaskInterpolation;
		SetMaskInterpolation(0f);
	}
	
	private void OnDestroy()
	{
		PlayerState.OnStateTransition -= SetMaskInterpolation;
	}
	
	void SetMaskInterpolation(float interpolation)
	{
		playerDress.material.SetFloat("_CutoffHeight", Mathf.Lerp(0f, 4f, interpolation));
		maskHunter.material.SetFloat("_CutoffHeight", Mathf.Lerp(-5f, 15f, 1 - interpolation));
		maskPrey.material.SetFloat("_CutoffHeight", Mathf.Lerp(-5f, 15f, 1 - interpolation));
	}
}
