using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTouge : UserStage
{
    public void Haruna()
    {
        data = HARUNADATA.Split(',');
        SetRoad();
    }

    string HARUNADATA = "%START,000,000,000,%END";
}
