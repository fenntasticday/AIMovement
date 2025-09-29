using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownControl : MonoBehaviour
{
    public EnvironmentSetup evSetup;

    public TMPro.TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeShape()
    {
        if (dropdown.value == 0)
            evSetup.isSquare = false;
        if (dropdown.value == 1)
            evSetup.isSquare = true;
    }

    public void ChangeNum()
    {
        if (dropdown.value == 0)
            evSetup.generationAmount = 20;
        if (dropdown.value == 1)
            evSetup.generationAmount = 30;
        if (dropdown.value == 2)
            evSetup.generationAmount = 100;
    }
}
