using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipScript : MonoBehaviour
{
    public enum shipControls {
        left,
        right,
        thrust,
        slow
    }

    // starsystem
    SystemGen systemGen;
    RuntimeScript runtime;
    GameObject[] planets;
    GameObject star;

    // ship
    Rigidbody2D rb;
    float shipMass = 0.0001f;
    public float angle;
    public float thrust = 0f;

    // player
    bool startflag = true;

    // ship stats
    float thrustPower = 0.1f;
    float maneuverPower = 0.01f;

    // prefabs
    [SerializeField] GameObject Trajectory;
    [SerializeField] GameObject linePrefab;

    // lines
    GameObject bowLine;
    GameObject veloLine;
    public Vector2 bowDir;
    float lineWidth = 0.008f;

    // camera
    CameraScript cam;
    float mult;

    // text
    GameObject veloText;

    // timescale
    public Vector2 veloSave;
    Vector2 veloNow;
    bool tickFlag;

    // weapons
    [SerializeField] public GameObject weapons;
    public bool shipMode;

    // aim
    GameObject aimline;

    #region Singleton
    public static ShipScript instance;
    void Awake() {
        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cam = CameraScript.instance;
        runtime = RuntimeScript.instance;
        RegenShipCache();
        rb = this.GetComponent<Rigidbody2D>();
        Trajectory = GameObject.FindWithTag("trajectory");
        veloText = GameObject.FindWithTag("velotext");
        angle = 0f;

        veloLine = Instantiate(linePrefab, transform);
        bowLine = Instantiate(linePrefab, transform);
        var bowColor = new Color(0.3f,1f,0.8f);
        bowLine.GetComponent<LineRenderer>().startColor = bowColor;
        bowLine.GetComponent<LineRenderer>().endColor = bowColor;

        //spawning
        transform.position = Random.insideUnitCircle.normalized * systemGen.massEdgeRange;
        rb.AddForce(rotate(-transform.position, (Random.Range(0,2)*2-1)*Random.Range(0.6f,0.8f)));
        startflag = false;

        // aimline
        aimline = Instantiate(linePrefab, gameObject.transform);

    }

    void FixedUpdate()
    {
        if (systemGen.system.star != star) RegenShipCache();
        Vector2 allForce = Vector2.zero;
        if (veloSave.magnitude > 0.05) {
            bowDir = rotate(veloSave.normalized,angle);
        }
        rb.AddForce(bowDir*thrust);
        veloSave += (bowDir*Time.fixedDeltaTime*thrust);

        Trajectory.GetComponent<LineRenderer>().startWidth = lineWidth*2*mult;
        Trajectory.GetComponent<LineRenderer>().endWidth = lineWidth*2*mult;
        mult = cam.cameraZooms[cam.cameraLevel];

        if (!runtime.pause) {
            veloSave = rb.velocity;
            foreach (GameObject planet in planets) {
                allForce += Calculate(planet,false,transform.position);
            } allForce += Calculate(star,true,transform.position);
            rb.AddForce(allForce);
        }
        if (tickFlag) Predict(2000);
        tickFlag = !tickFlag;

        BowIndicate();
        VelocityIndicate();

        VeloTextUpdate();

        weapons.GetComponent<WeaponScript>().SetDir(-bowDir);

        transform.localScale = new Vector3(0.015f*mult,0.015f*mult);
        thrust = 0f;

        if (shipMode) {
            aimline.SetActive(true);
            AimMouse();
        } else {
            aimline.SetActive(false);
        }
    }

    public void ShipPause(bool pause) {
        if (pause) {
                veloSave = rb.velocity;
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            } else {
                rb.isKinematic = false;
                rb.velocity = veloSave;
            }
    }

    public void RegenShipCache() {
        systemGen = SystemGen.instance;
        planets = systemGen.system.planets;
        star = systemGen.system.star;
    }

    public void ShipControl(shipControls Code, bool slow) {
        if(Code == shipControls.left) {
            if (slow) {
                angle += 0.1f*-maneuverPower;
            } else {
                angle += -maneuverPower;
            }
        }
        if(Code == shipControls.right) {
            if (slow) {
                angle += 0.1f*maneuverPower;
            } else {
                angle += maneuverPower;
            }
        }
        if(Code == shipControls.thrust) {
            thrust = thrustPower;
        }
    }

    public static Vector2 rotate(Vector2 v, float delta) {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    public Vector2 Calculate(GameObject mass, bool isStar, Vector2 pos) {
        var massScript = mass.GetComponent<MassScript>();
        float d2 = Mathf.Pow(Mathf.Max(massScript.Scale,Vector3.Distance(mass.transform.position,pos)),2);
        float G = 6.6743f*Mathf.Pow(10,2);
        float F = 0f;
        if (isStar) {
             F = (G*massScript.Mass*300*shipMass)/d2;
        } else {
            F = (G*massScript.Mass*shipMass)/d2;
        }
        Vector2 heading = (mass.transform.position-new Vector3(pos.x,pos.y,0));
        Vector2 Fdir = (F*(heading/heading.magnitude));
        return Fdir;
    }

    void BowIndicate() {
        var bowRend = bowLine.GetComponent<LineRenderer>();
        var point = FindMidPoint(-bowDir,3);
        bowRend.SetPosition(0, point);
        bowRend.SetPosition(1, point+(-bowDir)*2);
        bowRend.startWidth = lineWidth*mult;
        bowRend.endWidth = lineWidth*mult;
    }

    void VelocityIndicate() {
        var veloRend = veloLine.GetComponent<LineRenderer>();
        var point = FindMidPoint(-veloSave,3);
        veloRend.SetPosition(0, (point));
        veloRend.SetPosition(1, (point-veloSave*2));
        veloRend.startWidth = lineWidth*mult;
        veloRend.endWidth = lineWidth*mult;
    }

    Vector2 FindMidPoint(Vector2 P2, float midLength) {
        var P1 = Vector2.zero;
        Vector2 point = midLength * (P2- P1).normalized + P1;
        return point;
    }

    void VeloTextUpdate() {
        veloText.GetComponent<Text>().text = ((veloSave).magnitude*(6.233f*Mathf.Pow(10, 6))).ToString("0.00000E0") + "km/h";
    }

    void AimMouse() {
        var aimlineRend = aimline.GetComponent<LineRenderer>();
        aimlineRend.SetPosition(1,50/mult*(weapons.GetComponent<WeaponScript>().GetEnergy(runtime.massDriverPower)));
        aimlineRend.startWidth = lineWidth*mult;
        aimlineRend.endWidth = lineWidth*mult;
        aimlineRend.startColor = new Color(0.3f,0.2f,0.8f,0.8f);
        aimlineRend.endColor = new Color(0.5f,0.2f,0.8f,0.1f);
        /*
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector2 mouseline = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
        Vector2 point = FindMidPoint(mouseline, 10);*/
        /*
        if (shipMode) {
            var angleRef = AnglePoint(-bowDir-(Vector2)transform.position, point-(Vector2)transform.position);
            angle = angleRef;
            //var angleRef = Vector3.SignedAngle(-bowDir+(Vector2)transform.position, point+(Vector2)transform.position, Vector3.up)*Mathf.Deg2Rad;
            Debug.Log(angleRef);
            //Debug.DrawLine(transform.position, rotate(point+(Vector2)transform.position, angleRef));
            //angle = Vector3.SignedAngle(-bowDir, point, Vector3.up)*Mathf.Deg2Rad;
            Debug.DrawLine(transform.position, point+(Vector2)transform.position);
            Debug.DrawLine(transform.position, -bowDir+(Vector2)transform.position);
        }*/
        /*
        float AnglePoint(Vector2 a, Vector2 b) {
            return Mathf.Atan2(a.y - b.y, a.x - b.x);
        }
        float AnglePoint(Vector2 vec1, Vector2 vec2) {
            Vector2 dir = (vec1-vec2);
            return Mathf.Atan2(dir.y, dir.x);
        }*/

    }

    public void Predict(int stepCount) {
        var currentPos = rb.position;
        var prevPos = currentPos;
        var currentVelocity = veloSave;
        var planetCords = gameObject.transform.position;
        Vector3[] poslist = new Vector3[stepCount];

        for (int i = 0; i < stepCount; i++) {
            //var distance = planetCords - currentPos;

            //var forceMag = Gravity / distance.sqrMagnitude;
            //forces = distance.normalized * forceMag;
            Vector2 allForce = Vector2.zero;
            foreach (GameObject planet in planets) {
                allForce += Calculate(planet,false,currentPos);
            } allForce += Calculate(star,true,currentPos);

            currentVelocity  += allForce * Time.fixedDeltaTime;
            currentPos += currentVelocity * Time.fixedDeltaTime;

            //Debug.DrawLine(prevPos, currentPos, Color.red, Time.deltaTime);
            poslist[i] = currentPos;
            prevPos = currentPos;
        }
        var tline = Trajectory.GetComponent<LineRenderer>();
        tline.positionCount = stepCount;
        tline.SetPositions(poslist);
    }
}
