using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacer : MonoBehaviour
{
	[SerializeField] int trapsMaxNumber = 2;

	[SerializeField] Vector3 trapOffset;
	List<PlayerGhost> activeTraps = new List<PlayerGhost>();

    private void Update()
    {
	    if(Input.GetKeyDown(KeyCode.LeftShift) && PlayerState.IsPrey)
		    PlaceTrap();
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
