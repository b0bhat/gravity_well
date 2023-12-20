using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public bool massMode = false;
    ShipScript ship;
    [SerializeField] float angle;

    Vector2 bowDir;

    // round types
    [SerializeField] GameObject MassDriverRound;


    // Start is called before the first frame update
    void Start() {
        ship = ShipScript.instance;
    }

    // Update is called once per frame
    void Update() {
        //rotate(transform.eulerAngles,angle);
    }

    public void SetAngle(float angleSet) {
        angle = angleSet;
    }

    public void FireMassDriver(float power) {
        var thisRound = Instantiate(MassDriverRound);
        thisRound.transform.position = transform.position;
        thisRound.GetComponent<ProjectileScript>().Fire(power, bowDir, ship.veloSave);
    }

    public Vector2 GetEnergy(float power) {
        return (Mathf.Sqrt(2*power/MassDriverRound.GetComponent<ProjectileScript>().projMass)*(bowDir))+ship.veloSave;
    }

    public void SetDir(Vector2 dir) {
        bowDir = dir;
        transform.rotation = Quaternion.LookRotation(dir);

    }

    public static Vector2 rotate(Vector2 v, float delta) {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
}
