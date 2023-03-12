using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchCode : MonoBehaviour
{
    public Dictionary<string, Color> ColorDictionary = new Dictionary<string, Color>(){
            {"A1", new Color(221, 96, 96, 255) },
            {"B1", new Color(250, 155, 0, 255) },
            {"C1", new Color(231, 211, 0, 255) },
            {"D1", new Color(122, 184, 77, 255)},
            {"E1", new Color(91, 216, 255, 255)},
            {"F1", new Color(156, 119, 255, 255)},
            {"G1", new Color(221, 24, 255, 255)}
        };


}
