using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private bool bLeftShiftDown;
    private bool bFourDown;
    private bool bRDown;
    private bool bMDown;
    private bool bLDown;

    // Update is called once per frame
    void Update()
    {
        // Check if 'shift' key is pressed or released
        if (Input.GetKeyDown(KeyCode.LeftShift)) bLeftShiftDown = true;
        if (Input.GetKeyUp(KeyCode.LeftShift)) bLeftShiftDown = false;

        // Check if the '4' key is pressed or released.
        if (Input.GetKeyDown(KeyCode.Alpha4)) bFourDown = true;
        if (Input.GetKeyUp(KeyCode.Alpha4)) bFourDown = false;

        // Check if the 'r' key is pressed or released.
        if (Input.GetKeyDown(KeyCode.R)) bRDown = true;
        if (Input.GetKeyUp(KeyCode.R)) bRDown = false;

        // Check if the 'M' key is pressed or released.
        if (Input.GetKeyDown(KeyCode.M)) bMDown = true;
        if (Input.GetKeyUp(KeyCode.M)) bMDown = false;

        // Check if the 'L' key is pressed or released.
        if (Input.GetKeyDown(KeyCode.L)) bLDown = true;
        if (Input.GetKeyUp(KeyCode.L)) bLDown = false;

        // If 'shift' and '4' are pressed at the same time, increase money by $1,000,000
        if (bLeftShiftDown && bFourDown)
        {
            GameHandler.Instance.IncreaseMoney(1000000);
            bFourDown = false;
        }
        // If 'shift' and 'R' are pressed at the same time, increase the round
        if (bLeftShiftDown && bRDown && !GameHandler.Instance.GetComponent<RoundSpawning>().bInRound)
        {
            GameHandler.Instance.GetComponent<RoundSpawning>().ActivateRoundCheat();
            bRDown = false;
        }
        // If 'shift' and 'M' are pressed at the same time, give 100 of each module
        if (bLeftShiftDown && bMDown)
        {
            GameHandler.Instance.inventory.CheatAddItems();
            bMDown = false;
        }
        // If 'shift' and 'L' are pressed at the same time, toggle god mode
        if (bLeftShiftDown && bLDown)
        {
            GameHandler.Instance.bDebugMode = !GameHandler.Instance.bDebugMode;
            bMDown = false;
        }
    }
}
