using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacer : MonoBehaviour
{
	[SerializeField] int trapsMaxNumber = 2;
	[SerializeField] float trapCoolDown = .2f;

	[SerializeField] Vector3 trapOffset;
	List<PlayerGhost> activeTraps = new List<PlayerGhost>();

    private void Update()
    {
	    if(Input.GetKeyDown(KeyCode.LeftShift) && PlayerState.IsPrey && !isPlacingTrap)
        {
			isPlacingTrap = true;
			StartCoroutine(_PlaceTrap());
        }
    }

	bool isPlacingTrap = false;
	IEnumerator _PlaceTrap()
    {
		PlaceTrap();

		yield return new WaitForSeconds(trapCoolDown);

		isPlacingTrap = false;
	}

	void PlaceTrap()
	{
		var temp = PlayerGhostPooler.Spawn("DecoyTrap", transform.position + trapOffset, transform.rotation, null);
		temp.gameObject.GetComponent<DecoyTrap>().SetPlacer(this);

		activeTraps.Add(temp);

		if(activeTraps.Count > trapsMaxNumber)
        {
			activeTraps[0].Dissolve();
			activeTraps.RemoveAt(0);
        }
	}

	public void RemoveFromPlacer(PlayerGhost decoyTrap)
	{
		activeTraps.Remove(decoyTrap);
	}
}

public class Traps
{
	PlayerGhost[] _traps = new PlayerGhost[2];
	int index = 0;

	public Traps(int length)
    {
		_traps = new PlayerGhost[length];
	}

	public PlayerGhost Current
    {
        get { return _traps[index]; }
        set { _traps[index] = value; }
    }

	public void Next()
    {
		index++;

		if (index >= _traps.Length)
			index -= _traps.Length;
	}
}
