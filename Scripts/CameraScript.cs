using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    //camera speeds
    float baseSpeed = 2f;
    float timeSpeed = 0.02f;

    // camera zooms
    public float[] cameraZooms = new float[]{0.3f, 0.5f, 2f, 10f, 30f};
    public int cameraLevel = 3;

    //float lineWidth = 0.005f;
    [SerializeField] GameObject zoomLevel;
    [SerializeField] GameObject zoomType;
    [SerializeField] GameObject rect;
    [SerializeField] GameObject playerShip;


    //movements
    Vector3 dragOrigin;

    float camZoomDiff = 0f;
    bool focus = false;



    #region Singleton

    public static CameraScript instance;
    void Awake() {
        instance = this;
    }
    #endregion

    void Start()
    {
        cameraLevel = 4;
        Camera.main.orthographicSize = cameraZooms[cameraLevel];
        playerShip = GameObject.FindWithTag("playership");
        zoomType = GameObject.FindWithTag("zoomtype");
    }

    // Update is called once per frame
    void Update()
    {
        float speed = baseSpeed*cameraZooms[cameraLevel];
        if (!focus) {
            if(Input.GetKey(KeyCode.RightArrow)) {
                transform.Translate(new Vector3(speed * timeSpeed,0,0));
            }
            if(Input.GetKey(KeyCode.LeftArrow)) {
                transform.Translate(new Vector3(-speed * timeSpeed,0,0));
            }
            if(Input.GetKey(KeyCode.DownArrow)) {
                transform.Translate(new Vector3(0,-speed * timeSpeed,0));
            }
            if(Input.GetKey(KeyCode.UpArrow)) {
                transform.Translate(new Vector3(0,speed * timeSpeed,0));
            }
        }

        if(Input.GetKeyDown(KeyCode.F)) {
            focus = !focus;
            //Camera.main.orthographicSize = cameraZooms[cameraLevel];
        }
        if (focus) transform.position = new Vector3(playerShip.transform.position.x,playerShip.transform.position.y,-10);

        // [TODO] zoom to mouse
        //float zoom = cameraZooms[cameraLevel];
        //diff = Vector2.Distance(Input.mousePosition,Vector2(0,0))
        if(Input.GetKeyUp(KeyCode.Z)) {
            if (cameraLevel > 0) {
                cameraLevel--;
                camZoomDiff = 2*cameraZooms[cameraLevel+1]/cameraZooms[cameraLevel];
            }
            //Camera.main.orthographicSize = cameraZooms[cameraLevel];
        }

        if(Input.GetKeyDown(KeyCode.X)) {
            if (cameraLevel < cameraZooms.Length-1) {
                cameraLevel++;
                camZoomDiff = cameraZooms[cameraLevel]/cameraZooms[cameraLevel-1];
            }
            //Camera.main.orthographicSize = cameraZooms[cameraLevel];
        }
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, cameraZooms[cameraLevel], timeSpeed * speed*camZoomDiff);
        float zoom = cameraZooms[cameraLevel];
        zoomLevel.GetComponent<Text>().text = zoom.ToString();
        string zoomtypetext = "";
        if (cameraLevel == 0) zoomtypetext = "PDC Range";
        if (cameraLevel == 1) zoomtypetext = "Orbital Navigation";
        if (cameraLevel == 2) zoomtypetext = "Tactical Map";
        if (cameraLevel == 3) zoomtypetext = "Strategic Map";
        if (cameraLevel == 4) zoomtypetext = "System Model";
        zoomtypetext += "\n" + (4.3f*zoom).ToString() + " AUs";
        zoomType.GetComponent<Text>().text = zoomtypetext;
        if (!focus) PanCamera(speed);
        /* zoom indicator
        if(Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X)) {
            float height = 0f;
            Color color = new Color(0,0,0);
            if(Input.GetKey(KeyCode.Z)) {
                height = 2f*cameraZooms[Mathf.Max(0,cameraLevel-1)];
                color = new Color(0.6f,1f,0.8f,0.3f);
            }
            if(Input.GetKey(KeyCode.X)) {
                height = 2f*cameraZooms[Mathf.Min(cameraZooms.Length,cameraLevel-1)];
                color = new Color(1f,0.7f,0.7f,0.3f);
            }
            float width = height * Camera.main.aspect;
            rect.SetActive(true);
            Vector3[] rectPoints = new Vector3[5];
            LineRenderer rectLine = rect.GetComponent<LineRenderer>();
            rectLine.positionCount = 5;
            rectPoints[0] = (new Vector3(width/2,-height/2));
            rectPoints[1] = (new Vector3(width/2,height/2));
            rectPoints[2] = (new Vector3(-width/2,height/2));
            rectPoints[3] = (new Vector3(-width/2,-height/2));
            rectPoints[4] = (new Vector3(width/2,-height/2));
            rectLine.SetPositions(rectPoints);
            rectLine.startWidth = lineWidth*zoom;
            rectLine.endWidth = lineWidth*zoom;
            rectLine.SetColors(color,color);
        } else {
            rect.SetActive(false);
        }
             var pos = Camera.main.ScreenToViewportPoint(dragOrigin-Input.mousePosition);
            transform.Translate(dragOrigin-Input.mousePosition);
            dragOrigin = Input.mousePosition;
            //Debug.DrawLine(Input.mousePosition, dragOrigin);
        }*/
    }

    void PanCamera(float speed) {
        if (Input.GetMouseButtonDown(2)) {
            dragOrigin = Input.mousePosition;
        }
        if (Input.GetMouseButton(0)) {
            var pos = new Vector3();
            pos.x = Input.GetAxis("Mouse X") * 6*speed * timeSpeed;
            pos.y = Input.GetAxis("Mouse Y") * 6*speed * timeSpeed;
            transform.Translate(-pos);
        }
    }
}
