using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationDropDown : MonoBehaviour
{
    UnityEngine.UI.Dropdown _dropdown = null;

    private void Awake()
    {
        _dropdown = GetComponent<UnityEngine.UI.Dropdown>();
    }

    private void Start()
    {

    }
}
