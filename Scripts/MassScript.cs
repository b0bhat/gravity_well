using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MassScript : MonoBehaviour
{
    public enum massType {
        Star, Planet, Rock
    }

    // Material
    [SerializeField] Material starMaterial;
    [SerializeField] Material planetMaterial;

    // Prefabs
    [SerializeField] GameObject orbitPrefab;
    [SerializeField] GameObject innerPrefab;
    [SerializeField] GameObject iconPrefab;
    [SerializeField] GameObject pointLightPrefab;
    [SerializeField] GameObject colonyPrefab;
    [SerializeField] GameObject strikePrefab;

    // Text
    public GameObject classText;
    public GameObject massText;

    // Local Variables
    GameObject orbit;
    GameObject inner;
    GameObject select;
    GameObject pointLight;
    CameraScript cam;
    RuntimeScript runtime;

    // Mass Class and Type
    [SerializeField] public massType type;
    [SerializeField] public string Class;

    // Mass Information
    [SerializeField] public float Lumi;
    [SerializeField] public float Temp;
    [SerializeField] public float Mass;
    [SerializeField] public Color32 Chro;
    [SerializeField] public float Scale;
    [SerializeField] public float distance;

    // colony
    [SerializeField] GameObject colony;
    public bool habitable;

    // variables
    float lineWidth = 0.003f;
    public float rotation;
    bool clickFlag = false;
    bool mouseFlag = false;
    float mult;

    void Awake() {
        classText = GameObject.FindWithTag("classtext");
        massText = GameObject.FindWithTag("masstext");
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = CameraScript.instance;
        runtime = RuntimeScript.instance;
        gameObject.GetComponent<SpriteRenderer>().color = Chro;
        transform.localScale = new Vector3(Scale,Scale);
        if (type == massType.Planet) {
            // location, rotation
            gameObject.transform.position = new Vector3(distance,0,0);
            transform.localScale = new Vector3(Scale/10,Scale/10);
            rotation = Random.Range(0f,360f);
            transform.RotateAround(Vector3.zero, Vector3.forward, rotation);
            gameObject.transform.rotation = Quaternion.identity;

            // orbit
            orbit = Instantiate(orbitPrefab, SystemGen.instance.transform);
            MakeCircle(orbit.GetComponent<LineRenderer>(), distance, 500);

            // inner (or icon)
            inner = Instantiate(iconPrefab, transform);
            MakeTriangle(inner.GetComponent<LineRenderer>(), 0.5f);
            inner.transform.rotation = Quaternion.identity;

            // colony
            if (habitable) {
                colony = Instantiate(colonyPrefab, transform);
                inner.GetComponent<LineRenderer>().endColor = new Color(0.2f, 0.7f, 0.5f, 0.6f);
                inner.GetComponent<LineRenderer>().startColor = new Color(0.2f, 0.7f, 0.5f, 0.6f);
            }

            // material
            transform.GetComponent<SpriteRenderer>().material = planetMaterial;
            //starMaterial.SetColor("_Color", new Vector4(Chro.r, Chro.g, Chro.b, 20f));

        } else if (type == massType.Star) {
            // light
            pointLight = Instantiate(pointLightPrefab, transform);
            pointLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().color = Chro;
            pointLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = Mathf.Pow(Lumi, 1f/15f)-0.5f;

            // material
            starMaterial.SetColor("_Color", new Vector4(Chro.r, Chro.g, Chro.b, 20f));
            transform.GetComponent<SpriteRenderer>().material = starMaterial;

            // inner
            inner = Instantiate(innerPrefab, transform);
            inner.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0.98f);
            inner.transform.localScale = Vector2.zero;

            // text
            massTextStar();
            classText.GetComponent<Text>().text = Class;

        }
        // select
        select = Instantiate(orbitPrefab, transform);
        var selectLine = select.GetComponent<LineRenderer>();
        MakeCircle(select.GetComponent<LineRenderer>(), 1.5f, 100);
        select.GetComponent<LineRenderer>().endColor = Chro;
        select.GetComponent<LineRenderer>().startColor = Chro;
        select.SetActive(false);

        //gameObject.GetComponent<CircleCollider2D>().radius = Scale/10;
    }

    // Update is called once per frame
    void Update()
    {
        mult = cam.cameraZooms[cam.cameraLevel];
        if (type == massType.Planet) {
            orbit.GetComponent<LineRenderer>().startWidth = lineWidth*mult;
            orbit.GetComponent<LineRenderer>().endWidth = lineWidth*mult;
            var spriteSize = this.GetComponent<SpriteRenderer>();
            var size = 0f;
            if (cam.cameraLevel == cam.cameraZooms.Length-1) {
                size = 5f;
                //transform.localScale = new Vector3(0.1f*mult,0.1f*mult);
                transform.localScale = new Vector3(Mathf.Sqrt(Scale/size),Mathf.Sqrt(Scale/size));
                //spriteSize.size = new Vector3(Scale/size,Scale/size);
                this.GetComponent<CircleCollider2D>().radius = (Scale/30f)/transform.localScale.x;
                MouseCheck(size/2);
                InnerCamSet(mult);
                inner.SetActive(true);
            } else if (cam.cameraLevel == cam.cameraZooms.Length-2) {
                size = 8f;
                transform.localScale = new Vector3(Mathf.Sqrt(Scale)/size,Mathf.Sqrt(Scale)/size);
                //spriteSize.size = new Vector3(Mathf.Sqrt(Scale)/size,Mathf.Sqrt(Scale)/size)*5;
                this.GetComponent<CircleCollider2D>().radius = (Scale/30f)/transform.localScale.x;
                MouseCheck(size/10);
                InnerCamSet(mult);
                inner.SetActive(true);
            } else if (cam.cameraLevel == cam.cameraZooms.Length-3) {
                size = 12f;
                transform.localScale = new Vector3(Mathf.Sqrt(Scale)/size,Mathf.Sqrt(Scale)/size);
                //spriteSize.size = new Vector3(Mathf.Sqrt(Scale)/size,Mathf.Sqrt(Scale)/size)*5;
                this.GetComponent<CircleCollider2D>().radius = (Scale/30f)/transform.localScale.x;
                MouseCheck(2*Scale/size);
                InnerCamSet(mult);
                inner.SetActive(true);
            } else {
                size = 15f;
                transform.localScale = new Vector3(Scale/size,Scale/size);
                //spriteSize.size = new Vector3(Scale/size,Scale/size)*5;
                this.GetComponent<CircleCollider2D>().radius = (Scale/30f)/transform.localScale.x;
                MouseCheck(2*Scale/size);
                inner.SetActive(false);
            }

        } else if (type == massType.Star) {
            if (cam.cameraLevel >= cam.cameraZooms.Length-1) {
                transform.localScale = new Vector3(Scale,Scale);
                inner.SetActive(false);
                MouseCheck(Scale);
            } else if (cam.cameraLevel == cam.cameraZooms.Length-2) {
                transform.localScale = new Vector3(Scale/1.5f,Scale/1.5f);
                inner.SetActive(false);
                MouseCheck(Scale);
            } else {
                transform.localScale = new Vector3(Scale/2,Scale/2);
                inner.transform.localScale = new Vector3(0.95f,0.95f);
                inner.SetActive(true);
                MouseCheck(Scale);
            }
        }
        select.GetComponent<LineRenderer>().startWidth = lineWidth*mult*5;
        select.GetComponent<LineRenderer>().endWidth = lineWidth*mult*5;

        /*
        if (runtime.select != gameObject) {
            clickFlag = false;
            select.SetActive(false);
            if (habitable) colony.GetComponent<ColonyScript>().TextUpdate(false);
        }*/

    }

    void InnerCamSet(float mult) {
        inner.transform.localScale = new Vector3(0.2f/transform.localScale.x*mult,0.2f/transform.localScale.y*mult);
        inner.GetComponent<LineRenderer>().startWidth = lineWidth*2*mult;
        inner.GetComponent<LineRenderer>().endWidth = lineWidth*2*mult;
    }

    void OnMouseOver() {
        //Debug.Log(Class);

    }

    void MouseCheck(float size) {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (Vector2.Distance(worldPos, transform.position) < size) {
            mouseFlag = true;
            if (type == massType.Star) massTextStar();
            if (type == massType.Planet) massTextPlanet();
            select.SetActive(true);
            if (habitable) colony.GetComponent<ColonyScript>().TextUpdate(true);
        } else if (mouseFlag) {
            mouseFlag = false;
            //if (clickFlag) return;
            Unselect();
            //runtime.UnselectText(gameObject);
        }
    }
    /*
    void OnMouseDown() {
        runtime.SelectChange(gameObject);
        clickFlag = true;
    }*/

    void OnMouseExit() {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        var boom = Instantiate(strikePrefab);
        boom.transform.position = other.gameObject.transform.position;
        CinemachineShake.Instance.ShakeCamera(0.001f*mult, .05f);
        var velsquared = Mathf.Pow((other.gameObject.GetComponent<Rigidbody2D>().velocity).magnitude,2f);
        var halfmass = other.gameObject.GetComponent<ProjectileScript>().projMass/2f;
        runtime.HitText(velsquared*halfmass*Mathf.Pow(10,17));

        Destroy(other.gameObject);

    }

    string massTextStar() {
        string temptext = ("Mass: " + Mass.ToString("F2") + " M☉ (Solar Mass)\nTemperature: " +
        Temp.ToString("F2") + " K\nLuminosity: "+ Lumi.ToString("F2") + " L☉\n");
        classText.GetComponent<Text>().text = Class;
        massText.GetComponent<Text>().text = temptext;
        //Debug.Log(temptext);
        return temptext;
    }
    string massTextPlanet() {
        string temptext = ("Mass: " + Mass.ToString("F2") + " M⊕ (Earth Mass)\nTemperature: " +
        Temp.ToString("F2") + " K\nDistance: " + distance.ToString("F2") + " AU\n");
        classText.GetComponent<Text>().text = Class;
        massText.GetComponent<Text>().text = temptext;
        //Debug.Log(temptext);
        return temptext;
    }

    public string GetText(bool isclass) {
        if (isclass) return Class;
        if (type == massType.Star) return massTextStar();
        if (type == massType.Planet) return massTextPlanet();
        else return null;
    }

    public void Unselect() {
        clickFlag = false;
        select.SetActive(false);
        massText.GetComponent<Text>().text ="";
        classText.GetComponent<Text>().text ="";
        if (habitable) colony.GetComponent<ColonyScript>().TextUpdate(false);
    }

    public void MakeCircle(LineRenderer larc, float radius, int seg) {
        larc.positionCount = seg;
        float angleFirst = 0;
        List<Vector3> arcPoints = new List<Vector3>();
        for (int k = 0; k < seg; k++) {
            float x = Mathf.Sin(Mathf.Deg2Rad * angleFirst) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angleFirst) * radius;

            arcPoints.Add(new Vector3(x,y));

            angleFirst += (360 / seg+1);
        }
        Vector3[] arcPoints2 = arcPoints.ToArray();
        larc.SetPositions(arcPoints2);
    }

    public void MakeTriangle(LineRenderer larc, float h) {
        larc.positionCount = 4;
        List<Vector3> arcPoints = new List<Vector3>();

        arcPoints.Add(new Vector2(0,(Mathf.Sqrt(3)/3)*h));
        arcPoints.Add(new Vector2(-h/2,-(Mathf.Sqrt(3)/6)*h));
        arcPoints.Add(new Vector2(h/2,-(Mathf.Sqrt(3)/6)*h));
        arcPoints.Add(new Vector2(0,(Mathf.Sqrt(3)/3)*h));
        Vector3[] arcPoints2 = arcPoints.ToArray();
        larc.SetPositions(arcPoints2);
    }
}
