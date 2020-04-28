using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSpikeImages : MonoBehaviour
{
    [SerializeField] Sprite[] allSpikeImages;
    [SerializeField] Sprite[] allCircuitImages;

    public Sprite GetSpikeImageByIndex(int index)
    {
        return allSpikeImages[index];
    }

    public Sprite GetSpikebyColorCornerAndState(string color, string position, string state)
    {
        switch (color)
        {
            case "blue":
                switch(position)
                {
                    case "bottomleft":
                        switch (state)
                        {
                            case "closed":
                                return allSpikeImages[3];
                            case "down":
                                return allSpikeImages[0];
                            case "left":
                                return allSpikeImages[1];
                            case "two":
                                return allSpikeImages[2];
                        }
                        break;
                    case "bottomright":
                        switch (state)
                        {
                            case "closed":
                                return allSpikeImages[7];
                            case "down":
                                return allSpikeImages[4];
                            case "right":
                                return allSpikeImages[5];
                            case "two":
                                return allSpikeImages[6];
                        }
                        break;
                    case "topleft":
                        switch (state)
                        {
                            case "closed":
                                return allSpikeImages[11];
                            case "up":
                                return allSpikeImages[9];
                            case "left":
                                return allSpikeImages[8];
                            case "two":
                                return allSpikeImages[10];
                        }
                        break;
                    case "topright":
                        switch (state)
                        {
                            case "closed":
                                return allSpikeImages[15];
                            case "up":
                                return allSpikeImages[13];
                            case "right":
                                return allSpikeImages[12];
                            case "two":
                                return allSpikeImages[14];                        }
                        break;
                }
                break;
            case "green":
                switch (position)
                {
                    case "bottomleft":
                        switch(state)
                        {
                            case "closed":
                                return allSpikeImages[19];
                            case "down":
                                return allSpikeImages[16];
                            case "left":
                                return allSpikeImages[17];
                            case "two":
                                return allSpikeImages[18];
                        }
                        break;
                    case "bottomright":
                        switch(state)
                        {
                            case "closed":
                                return allSpikeImages[23];
                            case "down":
                                return allSpikeImages[20];
                            case "right":
                                return allSpikeImages[21];
                            case "two":
                                return allSpikeImages[22];
                        }
                        break;
                    case "topleft":
                        switch (state)
                        {
                            case "closed":
                                return allSpikeImages[31];
                            case "up":
                                return allSpikeImages[29];
                            case "left":
                                return allSpikeImages[28];
                            case "two":
                                return allSpikeImages[30];
                        }
                        break;
                    case "topright":
                        switch (state)
                        {
                            case "closed":
                                return allSpikeImages[27];
                            case "up":
                                return allSpikeImages[25];
                            case "right":
                                return allSpikeImages[24];
                            case "two":
                                return allSpikeImages[26];
                        }
                        break;
                }
                break;
            case "red":
                switch (position)
                {
                    case "bottomleft":
                        switch (state)
                        {
                            case "closed":
                                return allSpikeImages[35];
                            case "down":
                                return allSpikeImages[32];
                            case "left":
                                return allSpikeImages[33];
                            case "two":
                                return allSpikeImages[34];
                        }
                        break;
                    case "bottomright":
                        switch (state)
                        {
                            case "closed":
                                return allSpikeImages[39];
                            case "down":
                                return allSpikeImages[36];
                            case "right":
                                return allSpikeImages[37];
                            case "two":
                                return allSpikeImages[38];
                        }
                        break;
                    case "topleft":
                        switch (state)
                        {
                            case "closed":
                                return allSpikeImages[43];
                            case "up":
                                return allSpikeImages[41];
                            case "left":
                                return allSpikeImages[40];
                            case "two":
                                return allSpikeImages[42];
                        }
                        break;
                    case "topright":
                        switch (state)
                        {
                            case "closed":
                                return allSpikeImages[47];
                            case "up":
                                return allSpikeImages[46];
                            case "right":
                                return allSpikeImages[44];
                            case "two":
                                return allSpikeImages[45];
                        }
                        break;
                }
                break;
        }
        return allSpikeImages[48];
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

    public Sprite GetCircuitImageByIndex(int index)
    {
        return allCircuitImages[index];
    }

    public Sprite GetCircuitImageByColorAndDirection(string color, string direction)
    {
        switch (color)
        {
            case "blue":
                switch(direction)
                {
                    case "left":
                        return allCircuitImages[0];
                    case "top":
                        return allCircuitImages[1];
                    case "right":
                        return allCircuitImages[2];
                    case "bottom":
                        return allCircuitImages[3];
                }
                break;
            case "green":
                switch(direction)
                {
                    case "left":
                        return allCircuitImages[4];
                    case "top":
                        return allCircuitImages[5];
                    case "right":
                        return allCircuitImages[6];
                    case "bottom":
                        return allCircuitImages[7];
                }
                break;
            case "red":
                switch(direction)
                {
                    case "left":
                        return allCircuitImages[8];
                    case "top":
                        return allCircuitImages[9];
                    case "right":
                        return allCircuitImages[10];
                    case "bottom":
                        return allCircuitImages[11];
                }
                break;
        }
        return allCircuitImages[12];
    }

    // Circuit Indexes:
        // 0-3: blue
            // 0: left
            // 1: top
            // 2: right
            // 3: bottom
        // 4-7: green
            // 4: left
            // 5: top
            // 6: right
            // 7: bottom
        // 8-11: red
            // 8: left
            // 9: top
            // 10: right
            // 11: bottom
        // 12: none
}
