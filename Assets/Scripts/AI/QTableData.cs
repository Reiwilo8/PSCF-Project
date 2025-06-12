using System;
using System.Collections.Generic;

[Serializable]
public class QEntry
{
    public string state;
    public float[] qValues;
}

[Serializable]
public class QTableWrapper
{
    public List<QEntry> entries = new List<QEntry>();
}