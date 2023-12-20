using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RuntimeScript : MonoBehaviour
{
    public int systemNum;
    [SerializeField] public GameObject select;
    ShipScript ship;

    public bool pause = false;
    bool impactTextWait = false;

    [SerializeField] GameObject classText;
    [SerializeField] GameObject massText;
    [SerializeField] GameObject modeText;
    [SerializeField] GameObject energyText;
    [SerializeField] GameObject joulesText;

    // stats
    public float massDriverPower = 0.000002f;


    #region Singleton

    public static RuntimeScript instance;
    void Awake() {
        instance = this;
        systemNum = 1;
        /*
        classText = GameObject.FindWithTag("classtext");
        massText = GameObject.FindWithTag("masstext");
        modeText = GameObject.FindWithTag("modetext");
        energyText = GameObject.FindWithTag("energytext");*/
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ship = ShipScript.instance;
        select = null;

        var timeChange = 0.75f;
        Time.timeScale = timeChange;
        Time.fixedDeltaTime = timeChange*0.02f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!impactTextWait) {
            energyText.GetComponent<Text>().text = "";
            joulesText.GetComponent<Text>().text = "";
        }

        bool slow;
        if(Input.GetKey(KeyCode.LeftShift)) {
            slow = true;
        } else {
            slow = false;
        }
        /*
        var mult = CameraScript.instance.cameraZooms[CameraScript.instance.cameraLevel];
        if(Input.GetKey(KeyCode.M)) {
            CinemachineShake.Instance.ShakeCamera(0.001f*mult, .05f);
        }*/
        if(Input.GetKeyDown(KeyCode.Space)) {
            pause = !pause;
            ship.ShipPause(pause);
        }
        if(Input.GetKey(KeyCode.D)) {
            ship.ShipControl(ShipScript.shipControls.left, slow);
        }
        if(Input.GetKey(KeyCode.A)) {
            ship.ShipControl(ShipScript.shipControls.right, slow);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            var timeChange = 0.02f;
            Time.timeScale = timeChange;
            Time.fixedDeltaTime = timeChange*0.02f;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            var timeChange = 0.1f;
            Time.timeScale = timeChange;
            Time.fixedDeltaTime = timeChange*0.02f;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            var timeChange = 0.5f;
            Time.timeScale = timeChange;
            Time.fixedDeltaTime = timeChange*0.02f;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)) {
            var timeChange = 1.5f;
            Time.timeScale = timeChange;
            Time.fixedDeltaTime = timeChange*0.02f;
        }


        var shipWeapons = ship.weapons.GetComponent<WeaponScript>();
        bool fireMode = shipWeapons.massMode;

        if (fireMode) {
            if(Input.GetKeyDown(KeyCode.S)) {
                shipWeapons.FireMassDriver(massDriverPower);
            }
        } else {
            if(Input.GetKey(KeyCode.W)) {
                ship.ShipControl(ShipScript.shipControls.thrust, false);
            }
        }

        if(Input.GetKeyDown(KeyCode.Tab)) {
            if (fireMode) {
                modeText.GetComponent<Text>().text = "PROPULSION";
                shipWeapons.massMode = false;
                ship.shipMode = false;
            } else {
                modeText.GetComponent<Text>().text = "ARTILLERY";
                shipWeapons.massMode = true;
                ship.shipMode = true;
            }
        }

    }

    public void HitText(float hit) {
        impactTextWait = true;
        StartCoroutine(ExplosionText(hit));
    }

    IEnumerator ExplosionText(float hit) {
        while(impactTextWait) {
            energyText.GetComponent<Text>().text = ((hit/(4.184*Mathf.Pow(10,12))).ToString("F2") + " Kiloton Impact");
            joulesText.GetComponent<Text>().text = hit.ToString("0.00000E0") + " J";
            yield return new WaitForSeconds(2);
            impactTextWait = false;
        }
    }

    public void SelectChange(GameObject newthing) {
        if (select != null) select.GetComponent<MassScript>().Unselect();
        select = newthing;
    }


    public void UnselectText(GameObject unselected) {
        //massText.GetComponent<Text>().text ="";
        //classText.GetComponent<Text>().text ="";
        //select = null;
        if (select == null) {
            massText.GetComponent<Text>().text ="";
            classText.GetComponent<Text>().text ="";
        } else {
            massText.GetComponent<Text>().text = select.GetComponent<MassScript>().GetText(false);
            classText.GetComponent<Text>().text = select.GetComponent<MassScript>().GetText(true);
        }
    }
}
