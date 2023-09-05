using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AirportPlacement : MonoBehaviour
{
    [Serializable]
    public class AirportData
    {
        public string Name;
        public float Latitude; // In degrees
        public float Longitude; // In degrees
    }

    [Header("References")]
    [SerializeField] public GameObject redDotPrefab; // Reference to your red dot prefab
    [SerializeField] public TextAsset airportDataJSON; // Attach the JSON file in the Inspector
    [SerializeField] public TextMeshProUGUI airportNameText; // Reference to the text box for displaying airport names

    [Header("Settings")]
    [SerializeField] private float scaleFactor = 200f;

    private List<AirportData> airports; // List to store airport data
    private GameObject hoveredAirport;

    private void Start()
    {
        LoadAirportDataFromJSON();

        if (airports.Count == 0)
        {
            Debug.LogError("No airport data loaded from JSON.");
            return;
        }

        // Place airports on the sphere
        foreach (var airport in airports)
        {
            Vector3 position = CalculatePositionFromLatLong(airport.Latitude, airport.Longitude);

            GameObject redDot = Instantiate(redDotPrefab, position, Quaternion.identity);

            // Parent the red dot to the sphere
            redDot.name = airport.Name;
            redDot.transform.parent = transform;

            // Add event handlers for mouse enter and exit
            redDot.AddComponent<AirportMouseHandler>().Initialize(this, airport.Name);
        }
    }

    private void LoadAirportDataFromJSON()
    {
        if (airportDataJSON != null)
        {
            airports = new List<AirportData>();
            airports.AddRange(JsonUtility.FromJson<AirportDataList>(airportDataJSON.text).Airports);
        }
        else
        {
            Debug.LogError("No JSON file attached in the Inspector.");
        }
    }

    private Vector3 CalculatePositionFromLatLong(float latitude, float longitude)
    {
        float phi = 90 - latitude;
        float theta = longitude + 180;

        float x = Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Cos(theta * Mathf.Deg2Rad);
        float y = Mathf.Cos(phi * Mathf.Deg2Rad);
        float z = Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Sin(theta * Mathf.Deg2Rad);

        return new Vector3(x, y, z) * scaleFactor;
    }

    public void OnAirportMouseEnter(string airportName)
    {
        airportNameText.text = airportName;
    }

    public void OnAirportMouseExit()
    {
        airportNameText.text = "";
    }
}

[Serializable]
public class AirportDataList
{
    public List<AirportPlacement.AirportData> Airports;
}
