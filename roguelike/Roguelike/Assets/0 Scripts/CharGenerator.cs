using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharGenerator : MonoBehaviour
{


    [ContextMenu("Generate Char")]
    public void GenerateChar()
    {
        GameObject CharObject = new GameObject();
        Instantiate(CharObject);
        CharObject.transform.name = "Char";
    }
}
