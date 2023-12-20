using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] public float projMass;

    SystemGen systemGen;
    RuntimeScript runtime;
    GameObject[] planets;
    GameObject star;
    CameraScript cam;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake() {
        cam = CameraScript.instance;
        systemGen = SystemGen.instance;
        runtime = RuntimeScript.instance;
        rb = this.GetComponent<Rigidbody2D>();
        RegenSystem();
        Destroy(gameObject, Random.Range(10.0f, 15.0f));
    }

    // Update is called once per frame
    void FixedUpdate() {
        var mult = cam.cameraZooms[cam.cameraLevel];
        Vector2 allForce = Vector2.zero;
        foreach (GameObject planet in planets) {
            allForce += Calculate(planet,false,transform.position);
        } allForce += Calculate(star,true,transform.position);
        rb.AddForce(allForce);

        this.GetComponent<SpriteRenderer>().size = new Vector2(mult*0.05f,0.10f*mult);
    }

    public void Fire(float energy, Vector2 dir, Vector2 velo) {
        Awake();
        rb = this.GetComponent<Rigidbody2D>();
        //Debug.Log((Mathf.Sqrt(2*energy/projMass)));
        rb.velocity = (Mathf.Sqrt(2*energy/projMass)*(dir.normalized))+velo;
    }

    void RegenSystem() {
        planets = systemGen.system.planets;
        star = systemGen.system.star;
    }

    public Vector2 Calculate(GameObject mass, bool isStar, Vector2 pos) {
        var massScript = mass.GetComponent<MassScript>();
        float d2 = Mathf.Pow(Mathf.Max(massScript.Scale,Vector3.Distance(mass.transform.position,pos)),2);
        if (d2 > 20) {return Vector2.zero;}
        float G = 6.6743f*Mathf.Pow(10,2);
        float F = 0f;
        if (isStar) {
             F = (G*massScript.Mass*300*projMass)/d2;
        } else {
            F = (G*massScript.Mass*projMass)/d2;
        }
        Vector2 heading = (mass.transform.position-new Vector3(pos.x,pos.y,0));
        Vector2 Fdir = (F*(heading/heading.magnitude));
        return Fdir;
    }

}
