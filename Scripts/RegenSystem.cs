using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenSystem : MonoBehaviour
{
    SystemGen systemGen;
    RuntimeScript runtime;
    GameObject planetHolder;
    // Start is called before the first frame update
    void Start()
    {
        systemGen = SystemGen.instance;
        runtime = RuntimeScript.instance;
        planetHolder = GameObject.FindWithTag("planetholder");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonPress() {
        foreach (Transform child in systemGen.transform) {
            GameObject.Destroy(child.gameObject);
        } foreach (Transform child in planetHolder.transform) {
            GameObject.Destroy(child.gameObject);
        } systemGen.CreateSystem();
        runtime.systemNum++;

    }
}
