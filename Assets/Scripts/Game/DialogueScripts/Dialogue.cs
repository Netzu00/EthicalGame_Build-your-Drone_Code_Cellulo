using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//want it show up in inspector
[System.Serializable]
public class Dialogue {
    [TextArea(3,10)] // (min_num_lines, max_num_lines)
    public string[] sentences;
}