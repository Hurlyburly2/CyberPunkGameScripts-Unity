using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] string color = "none";
    [SerializeField] string state = "none";
    // Spike Colors
        // red, blue, green
    // Spike States
        // none
        // closed
        // left
        // top
        // right
        // bottom
        // topAndLeft
        // topAndRight
        // bottomAndLeft
        // bottomAndRight

    public void SetupSpike()
    {

    }

    // spike image reference:
        // 0-15: blue
            // 0-3: Bottom Left
                // 0: Down Connection
                // 1: Left Connection
                // 2: Two Connection
                // 3: No Connection
            // 4-7: Bottom Right
                // 4: Down Connection
                // 5: Right Connection
                // 6: Two Connection
                // 7: No Connection
            // 8-11: Top Left
                // 8: Left Connection
                // 9: Top Connection
                // 10: Two Connection
                // 11: No Connection
            // 12-15: Top Right
                // 12: Right Connection
                // 13: Top Connection
                // 14: Two Connection
                // 15: No Connection
        // 16-31: GREEN
            // 16-19: Bottom Left
                // 16: Down Connection
                // 17: Left Connection
                // 18: Two Connections
                // 19: No Connection
            // 20-23: Bottom Right
                // 20: Down Connection
                // 21: Right Connection
                // 22: Two Connections
                // 23: No Connection
            // 24-27: Top Right
                // 24: Right Connection
                // 25: Top Connection
                // 26: Two Connections
                // 27: No Connection
            // 28-31: Top Left
                // 28: Left Connection
                // 29: Top Connection
                // 30: Two Connections
                // 31: No Connection
        // 32-47: RED
            // 32-35: Down Left
                // 32: Down Connection
                // 33: Left Connection
                // 34: Two Connections
                // 35: No Connection
            // 36-39: Down Right
                // 36: Down Connection
                // 37: Right Connection
                // 38: Two Connections
                // 39: No Connection
            // 40-43: Top Left
                // 40: Left Connection
                // 41: Top Connection
                // 42: Two Connections
                // 43: No Connections
            // 44-47: Top Right
                // 44: Right Connection
                // 45: Two Connections
                // 46: Up Connection
                // 47: No Connections
}
