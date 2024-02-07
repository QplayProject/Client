using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test: MonoBehaviour
{
    private void Start()
    {
        var version = GameManager.Instance.VersionCheck;
        Debug.Log($"{version}");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
