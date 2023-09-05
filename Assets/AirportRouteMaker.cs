using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirportRouteMaker : MonoBehaviour
{
    [Header("Link to the Flight Tracker (Required)")]
    [SerializeField]
    private GameObject flightTracker;

    public int numPoints = 50;
    private LineRenderer lineRenderer;

    private List<Transform> currentAirports;
    private List<string> currentRouteString;
    private Transform previouslySelectedDAirport;
    private Transform previouslySelectedAAirport;

    private Dictionary<string, Material> airportMaterials = new Dictionary<string, Material>();

    void Start()
    {
        currentAirports = new List<Transform>();
        flightTracker = GameObject.Find("Flight Tracker");
        StoreInitialMaterials();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;
    }

    void Update()
    {
        currentRouteString = flightTracker.GetComponent<RandomFlightInfo>().GetCurrentRoute(); // Index 0 is arrival, 1 is departure

        if (currentRouteString[0] != "")
        {
            ChangeAirportMaterials(currentRouteString[1], currentRouteString[0]);
        }
    }

    void DrawLine(Transform startPoint,Transform endPoint)
    {
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (float)(numPoints - 1);
            Vector3 point = Vector3.Slerp(startPoint.position, endPoint.position, t);
            lineRenderer.SetPosition(i, point);
        }
    }

    public void ClearLine()
    {
        lineRenderer.enabled=false;
    }

    void StoreInitialMaterials()
    {
        airportMaterials.Clear();
        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                airportMaterials[child.name] = renderer.material;
            }
        }
    }

    void ChangeAirportMaterials(string departure, string arrival)
    {
        // Revert previously selected airports to their initial material
        RevertAirportMaterial(previouslySelectedDAirport);
        RevertAirportMaterial(previouslySelectedAAirport);

        foreach (Transform child in transform)
        {
            if (child.name == departure || child.name == arrival)
            {
                if(!currentAirports.Contains(child) )
                {
                    Debug.Log("Drawing trail");
                    currentAirports.Add(child);
                }
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Store the previously selected airports
                    if (child.name == departure)
                    {
                        previouslySelectedDAirport = child;
                    }
                    else if (child.name == arrival)
                    {
                        previouslySelectedAAirport = child;
                    }

                    // Change the material to yellow with high emission
                    Material airportMaterial = new Material(renderer.material);
                    airportMaterial.color = Color.yellow;
                    airportMaterial.EnableKeyword("_EMISSION");
                    airportMaterial.SetColor("_EmissionColor", Color.yellow * 2.0f); // Adjust emission intensity
                    renderer.material = airportMaterial;
                }
            }
        }
        if(currentAirports!=null && currentAirports.Count==2)
        {
            lineRenderer.enabled=true;
            DrawLine(currentAirports[0],currentAirports[1]);

        }
        else
        {
            ClearLine();
        }
    }

    void RevertAirportMaterial(Transform airport)
    {
        if(currentAirports!=null)
        {
            currentAirports.Clear();
        }
        if (airport != null)
        {
            Renderer renderer = airport.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material initialMaterial;
                if (airportMaterials.TryGetValue(airport.name, out initialMaterial))
                {
                    renderer.material = initialMaterial;
                }
            }
 
        }
    }
}
