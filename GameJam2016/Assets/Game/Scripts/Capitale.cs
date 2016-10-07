using UnityEngine;
using System.Collections;

public class Capitale : Village {

    public int capitaleOr = 10;
    public int capitaleNourriture = 20;
    public int capitaleArmy = 5;
    public int bonheur = 100;

    public Capitale(Empire empire) : base(empire)
    {
        this.empire = empire;
        or = capitaleOr;
        nourriture = capitaleNourriture;
        army = capitaleArmy;
}

	void Start () {
	
	}
	
	void Update () {
	
	}
}
