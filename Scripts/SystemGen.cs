using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGen : MonoBehaviour
{
    public struct starSystem {
        public GameObject[] planets {get; set;}
        public GameObject star {get; set;}
        public string starType {get; set;}
        public starSystem(GameObject[] planets, GameObject star, string starType) {
            this.planets = planets;
            this.star = star;
            this.starType = starType;
        }
    }

    [SerializeField] GameObject massPrefab;
    [SerializeField] public starSystem system;
    [SerializeField] GameObject PlanetHolder;
    int numPlanets;
    public float massEdgeRange;

    List<float> distances = new List<float>();

    #region Singleton

    public static SystemGen instance;
    void Awake() {
        instance = this;
        PlanetHolder = GameObject.FindWithTag("planetholder");
        CreateSystem();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateSystem() {
        distances = new List<float>();
        GameObject star = CreateStar();
        float starRadius = star.GetComponent<MassScript>().Scale;
        float starTemp = star.GetComponent<MassScript>().Temp;

        //planets
        int numPlanets = Random.Range(2,10);
        GameObject[] planets = new GameObject[numPlanets];
        planets[0] = CreatePlanet(true, starRadius, starTemp);
        for (int i = 1; i < numPlanets; i++) {
            planets[i] = CreatePlanet(false, starRadius, starTemp);
        }


        system = new starSystem(planets,star,star.GetComponent<MassScript>().Class);
        massEdgeRange = Random.Range(10f,15f) + Mathf.Max(distances.ToArray());

    }

    GameObject CreateStar() {
        float Lumi=0f;
        float Temp=0f;
        float Mass=0f;
        float Scale=0f;
        Color32 Chro=new Color32(0,0,0,1);
        string starType;
        int starChance = Random.Range(0,100);
        var rand = Random.Range(0f,1f);
        if (starChance < 5) {                           starType = "Class O";
            Lumi = Random.Range(30000f, 50000f);
            Temp = Random.Range(30000f, 50000f);
            starType +=  (10*Mathf.InverseLerp(30000f,50000f,Temp)).ToString("F0");
            starType +=  "I Supergiant";
            Mass = Random.Range(16f, 30f);
            Chro = new Color32(146,168,255,255);
            Scale = Random.Range(2.6f, 4);
        } else if (starChance >= 5 && starChance < 12) { starType = "Class B";
            Lumi = Random.Range(25f, 30000f);
            Temp = Random.Range(10000f, 30000f);
            starType +=  (10*Mathf.InverseLerp(10000f,30000f,Temp)).ToString("F0");
            starType +=  "II Giant";
            Mass = Random.Range(2.1f, 16f);
            Chro = new Color32(161,182,253,255);
            Scale = Random.Range(1.8f, 2.6f);
        } else if (starChance >= 12 && starChance < 20) { starType = "Class A";
            Temp = Random.Range(7500f, 10000f);
            starType +=  (10*Mathf.InverseLerp(7500f, 10000f,Temp)).ToString("F0");
            if (rand > 0.2) {
                Lumi = Random.Range(5f, 15f);
                starType +=  "V Main Sequence";
                Scale = Random.Range(1.4f, 1.8f);
            } else {
                Lumi = Random.Range(15f, 25f);
                starType +=  "I Supergiant";
                Scale = Random.Range(2f, 4f);
            }
            Mass = Random.Range(1.4f, 2.1f);
            Chro = new Color32(200,210,254,255);
        } else if (starChance >= 20 && starChance < 32) { starType = "Class F";
            Lumi = Random.Range(1.5f, 5f);
            Temp = Random.Range(6000f, 7500f);
            starType +=  (10*Mathf.InverseLerp(6000f, 7500f,Temp)).ToString("F0");
            if (rand > 0.2) {
                Lumi = Random.Range(0.6f, 1.5f);
                starType +=  "V Main Sequence";
                Scale = Random.Range(1.15f, 1.4f);
            } else if (rand > 0.10) {
                Lumi = Random.Range(4f, 10f);
                starType +=  "II/III Giant";
                Scale = Random.Range(2f, 3.5f);
            } else {
                Lumi = Random.Range(10f, 25f);
                starType +=  "I Supergiant";
                Scale = Random.Range(3.5f, 5);
            }
            Mass = Random.Range(1.04f, 1.4f);
            Chro = new Color32(248,247,255,255);
        } else if (starChance >= 32 && starChance < 60) { starType = "Class G";
            Temp = Random.Range(5200f, 6000f);
            starType +=  (10*Mathf.InverseLerp(5200f, 6000f,Temp)).ToString("F0");
            if (rand > 0.3) {
                Lumi = Random.Range(0.6f, 1.5f);
                starType +=  "V Main Sequence";
                Scale = Random.Range(0.96f, 1.15f);
            } else if (rand > 0.15) {
                Lumi = Random.Range(3f, 8f);
                starType +=  "II/III Giant";
                Scale = Random.Range(2f, 3.5f);
            } else {
                Lumi = Random.Range(8f, 25f);
                starType +=  "I Supergiant";
                Scale = Random.Range(3.5f, 5f);
            }
            Mass = Random.Range(0.8f, 1.04f);
            Chro = new Color32(248,230,160,255);
        } else if (starChance >= 60 && starChance < 75) { starType = "Class K";
            Temp = Random.Range(3700f, 5200f);
            starType +=  (10*Mathf.InverseLerp(3700f, 5200f,Temp)).ToString("F0");
            if (rand > 0.3) {
                Lumi = Random.Range(0.08f, 0.6f);
                starType +=  "V Main Sequence";
                Scale = Random.Range(0.7f, 0.96f);
            } else if (rand > 0.15) {
                Lumi = Random.Range(2f, 6f);
                starType +=  "II/III Giant";
                Scale = Random.Range(1f, 2f);
            } else {
                Lumi = Random.Range(6f, 25f);
                starType +=  "I Supergiant";
                Scale = Random.Range(2f, 4f);
            }
            Mass = Random.Range(0.45f, 0.08f);
            Chro = new Color32(254,210,164,255);
        } else if (starChance >= 75 && starChance < 90) { starType = "Class M";
            Temp = Random.Range(2400f, 3700f);
            starType +=  (10*Mathf.InverseLerp(2400f, 3700f,Temp)).ToString("F0");
            if (rand > 0.3) {
                Lumi = Random.Range(0.08f, 0.01f);
                starType +=  "V Main Sequence";
                Scale = Random.Range(0.5f, 0.7f);
            } else if (rand > 0.15) {
                Lumi = Random.Range(1f, 5f);
                starType +=  "II/III Giant";
                Scale = Random.Range(1f, 2f);
            } else {
                Lumi = Random.Range(5f, 25f);
                starType +=  "I Supergiant";
                Scale = Random.Range(2f, 4f);
            }
            Mass = Random.Range(0.08f, 0.45f);
            Chro = new Color32(254,140,118,255);
        } else {                                          starType = "Class D";
            Temp = Random.Range(3000f, 60000f);
            starType +=  (10*Mathf.InverseLerp(3000f, 60000f,Temp)).ToString("F0");
            Lumi = Random.Range(0.01f, 0.001f);
            starType +=  "VII White Dwarf";
            Mass = Random.Range(1.5f, 0.8f);
            Chro = new Color32(255,240,240,255);
            Scale = Random.Range(0.5f, 0.3f);
        }
        GameObject massStar = Instantiate(massPrefab, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
        MassScript m  = massStar.GetComponent<MassScript>();
        m.type = MassScript.massType.Star;
        m.Lumi = Lumi;
        m.Temp = Temp;
        m.Mass = Mass;
        m.Chro = Chro;
        m.Scale = Scale;
        m.Class = starType;
        return massStar;
    }

    GameObject CreatePlanet(bool Habitable, float starRadius, float starTemp) {
        string planetType;
        //float Lumi=0f;
        //float Temp=0f;
        float Mass=0f;
        float Scale=0f;
        float Distance = 0f;
        float Temp = 0f;
        Color32 Chro=new Color32(0,0,0,1);
        float Phase = Random.Range(0f,0.2f);
        float Tint = Random.Range(-0.07f,0.07f);
        int planetChance = Random.Range(0,100);
        if (Habitable) planetChance = Random.Range(65,100);
        if (planetChance < 25) {
            planetType = "Gas Giant";
            Mass = Random.Range(100f,800f);
            Scale = Random.Range(0.8f,1.0f)*((Mass/200)+1.2f);
            Chro = new Color(0.75f+Phase,0.6f+Phase+Tint,0.4f+Phase);
            if (Random.Range(0f,1f) > 0.6f) {
                Distance = Random.Range(3f,30f);
            } else {
                Distance = Random.Range(0.1f,3);
            }
            Temp = Random.Range(60f, 520f);
        } else if (planetChance >= 25 && planetChance < 40) {
            planetType = "Ice Giant";
            Mass = Random.Range(20f,80f);
            Scale = Random.Range(0.8f,1.0f)*((Mass/20)+0.8f);
            Chro = new Color(0.55f+Phase,0.6f+Phase+Tint,0.75f+Phase);
            Distance = Random.Range(8f,40f);
            Temp = Random.Range(100f, 200f);
        } else { //terrestrial
            if (Random.Range(0f,1f) > 0.8f) {
                Distance = Random.Range(3f,30f);
            } else {
                Distance = Random.Range(0.1f,3);
            }

            if (Random.Range(0f,1f) > 0.6f) {
                Mass = Random.Range(0.6f,1f);
            } else {
                Mass = Random.Range(1f,6f);
            }

            Scale = Random.Range(0.2f,0.3f)*(Mass)+0.5f;
            Temp = Random.Range(10f, 50f);
            if (planetChance >= 40 && planetChance < 45) {
                planetType = "Molten Planet";
                Chro = new Color(0.5f+Phase,0.4f+Phase+Tint,0.33f+Phase);
                Temp += Random.Range(100f, 400f);
                Distance = (Mathf.Max(Distance-Random.Range(0f, 2f),0.3f));
            } else if (planetChance >= 45 && planetChance < 50) {
                planetType = "Sulfuric Planet";
                Chro = new Color(0.78f+Phase,0.65f+Phase+Tint,0.4f+Phase);
                Temp += Random.Range(200f, 600f);
            } else if (planetChance >= 55 && planetChance < 60) {
                planetType = "Liquid Planet";
                Chro = new Color(0.6f+Phase,0.72f+Phase,0.68f+Phase+Tint);
                Temp += Random.Range(20f,200f);
            } else if (planetChance >= 65 && planetChance < 75) {
                planetType = "Frozen Planet";
                Chro = new Color(0.8f+Phase,0.87f+Phase+Tint,0.92f+Phase);
                Distance += Random.Range(3f, 12f);
            } else {
                planetType = "Rocky Planet";
                Chro = new Color(0.32f+Phase,0.26f+Phase+Tint,0.22f+Phase);
                Temp += Random.Range(0f,80f);
            }
        }
        Distance += starRadius;
        Distance = CheckOrbit(Distance);
        distances.Add(Distance);
        if (Habitable) {
            if (Distance > 10f) {
                Distance -= (Distance-10f)-Random.Range(0f,10f);
            } if (Distance < 1f) {
                Distance += Random.Range(0.8f,2f);
            }
        }

        Temp += ((Mathf.Sqrt(starTemp)+300)/Distance+1);

        GameObject planet = Instantiate(massPrefab, new Vector3(0,0,0), Quaternion.identity, PlanetHolder.transform);
        MassScript m  = planet.GetComponent<MassScript>();
        m.type = MassScript.massType.Planet;
        m.Mass = Mass;
        m.Chro = Chro;
        m.Scale = Scale;
        m.Temp = Temp;
        m.Class = planetType;
        m.distance = Distance;
        m.habitable = Habitable;
        return planet;
        /*
        if (Habitable && (Temp > 350 || Temp < 200)) {
            return CreatePlanet(Habitable, starRadius, starTemp);
        } else {

        }*/
    }

    float CheckOrbit(float Distance) {
        foreach (float distance in distances) {
            var distDiff = Mathf.Abs(Distance-distance);
            if (distDiff < 0.4f ) {
                Distance += Random.Range(distDiff+0.1f,7f);
                //Debug.Log("efer");
                Distance = CheckOrbit(Distance);
            }
        } return Distance;
    }

}
