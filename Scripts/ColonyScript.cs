using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColonyScript : MonoBehaviour
{
    public enum colonyType {
        Research, Trade, Military, Industry, Civilian
    }
    public enum colonyLocation {
        Station, Surface, Underground
    }
    public enum colonyTargets {
        Relay, Elevator, Spaceport,
        Base, Production, Facilities
    }

    // basic
    colonyType type;
    colonyLocation location;
    float pop; // exponential scale
    string colonyClass;
    string owner;
    RuntimeScript runtime;

    // defenses
    float resist;

    // targets
    List<colonyTargets> targets = new List<colonyTargets>();
    float Infrastructure;
    float maxInfra;

    // text
    GameObject colonyText;
    GameObject colonyInfoText;
    GameObject colonyTypeText;
    GameObject colonyTargetText;
    Text colonytext;
    Text infotext;
    Text typetext;
    Text targettext;

    // owner
    List<string> ownerList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        colonyText = GameObject.FindWithTag("colonytext");
        colonyInfoText = GameObject.FindWithTag("colonyinfotext");
        colonyTypeText = GameObject.FindWithTag("colonytype");
        colonyTargetText = GameObject.FindWithTag("colonytargets");

        colonytext = colonyText.GetComponent<Text>();
        infotext = colonyInfoText.GetComponent<Text>();
        typetext = colonyTypeText.GetComponent<Text>();
        targettext = colonyTargetText.GetComponent<Text>();

        colonytext.text = "";
        infotext.text = "";
        typetext.text = "";
        targettext.text = "";
        runtime = RuntimeScript.instance;

        CreateColony();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TextUpdate(bool mouseOver) {
        colonytext = colonyText.GetComponent<Text>();
        infotext = colonyInfoText.GetComponent<Text>();
        typetext = colonyTypeText.GetComponent<Text>();
        if (mouseOver) {
            string text = "";
            if (pop > 9) {
                text = "Ecumenopolis";
            } else if (pop > 7) {
                text = "Exopolis";
            } else if (pop > 6) {
                text = "Colony";
            } else if (pop > 5) {
                text = "Settlement";
            } else {
                text = "Outpost";
            }  string targetall = "Primary Targets:\n";
            foreach (colonyTargets target in targets) {
                targetall += target.ToString() + "\n";
            }
            typetext.text = type.ToString() + " - " + owner;
            colonytext.text = colonyClass + " " + text;
            infotext.text = "Population: " + Mathf.Pow(10, pop).ToString("F0")
                + "\nProtection: " + resist.ToString("F2") + "\nInfrastructure: "+ Infrastructure.ToString("F2");
            targettext.text = targetall;
            ;
        } else {
            colonytext.text = "";
            infotext.text = "";
            typetext.text = "";
            targettext.text = "";
        }
    }

    public void CreateColony() {
        var locationChance = Random.Range(0f,1f);
        var runs = runtime.systemNum/8f;
        pop += Random.Range(3.5f+runs/2f,4.5f+runs);

        var typeChance = Random.Range(0f,1f);
        if (typeChance > 0.85) {
            type = colonyType.Research;
            pop = pop/Random.Range(1.05f,1.2f);
        } else if (typeChance > 0.7) {
            type = colonyType.Military;
            pop = pop/Random.Range(1.03f,1.1f);
        } else if (typeChance > 0.6) {
            type = colonyType.Industry;
            pop = pop/Random.Range(0.9f,1.1f);
        } else if (typeChance > 0.4) {
            type = colonyType.Civilian;
            pop = pop/Random.Range(0.8f,0.9f);
        }

        Infrastructure = pop + Random.Range(-0.3f*pop,0.3f*pop);
        resist = pop + Random.Range(-0.5f*pop,0.5f*pop);
        SetTargets();
        if (location == colonyLocation.Station) resist *= Random.Range(0.1f,0.2f);
        owner = SetOwner();

    }

    void SetTargets() {
        if (type == colonyType.Research) {
            targets.Add(colonyTargets.Facilities);
            string[] classes = {"Tech", "Medical", "Exploration", "Education"};
            colonyClass = classes[Random.Range(0,classes.Length)];

        } else if (type == colonyType.Military) {
            targets.Add(colonyTargets.Base);
            resist += Random.Range(0.3f*pop,0.6f*pop);
            string[] classes = {"Command", "Training", "Shipyard", "Occupied"};
            colonyClass = classes[Random.Range(0,classes.Length)];

        } else if (type == colonyType.Industry) {
            targets.Add(colonyTargets.Production);
            Infrastructure += Random.Range(0.5f*pop,0.8f*pop);
            string[] classes = {"Mining", "Agriculture", "Refining", "Manufacturing"};
            colonyClass = classes[Random.Range(0,classes.Length)];

        } else if (type == colonyType.Civilian) {
            Infrastructure += Random.Range(0.1f*pop,0.3f*pop);
            resist += Random.Range(-0.3f*pop,-0.6f*pop);
            string[] classes = {"Finance", "Trade", "Media", "Tourism", "Residential", "Residential"};
            colonyClass = classes[Random.Range(0,classes.Length)];
        }

        if (pop > Random.Range(5f,9f)) {
            targets.Add(colonyTargets.Spaceport);
            return;
        } if (pop > Random.Range(4f,6f)) {
            targets.Add(colonyTargets.Elevator);
            return;
        } else {
            targets.Add(colonyTargets.Relay);
        }

    }

    string SetOwner() {
        string ownerName = "";
        var typeChance = Random.Range(0f,1f);
            if (typeChance > 0.97) ownerName = "First Dominion";
            else if (typeChance > 0.94) ownerName = "Commonwealth of Stars";
            else if (typeChance > 0.9) ownerName = "Verkasa Dominion";
            else if (typeChance > 0.85) ownerName = "Qhi Coalition";
            else if (typeChance > 0.8) ownerName = "Diaspora";
            else if (typeChance > 0.75) ownerName = "Imperial Houses";
            else if (typeChance > 0.6) ownerName = "Union of Outer Worlds";
            else if (typeChance > 0.5) ownerName = "Tau Collective";
            else if (typeChance > 0.37) ownerName = "Nonaligned";
            else if (typeChance > 0.22) ownerName = "Sol Union";
            else if (typeChance > 0) ownerName = "Orion Empire";
    return ownerName;
    }

}
