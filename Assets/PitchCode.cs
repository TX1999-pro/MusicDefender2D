using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchCode : MonoBehaviour
{
    public Dictionary<string, Color32> CodeBook = new Dictionary<string, Color32>(){
            {"A1", new Color32(221, 96, 96, 255) },
            {"B1", new Color32(250, 155, 0, 255) },
            {"C1", new Color32(231, 211, 0, 255) },
            {"D1", new Color32(122, 184, 77, 255)},
            {"E1", new Color32(91, 216, 255, 255)},
            {"F1", new Color32(156, 119, 255, 255)},
            {"G1", new Color32(221, 24, 255, 255)}
        };


}
