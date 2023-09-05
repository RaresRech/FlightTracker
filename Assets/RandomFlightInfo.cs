using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class FlightData
{
    public FlightInfo[] data;
}

[Serializable]
public class FlightAirport
{
    public string continent;
    public string airport;
    public string iata; 
    public string scheduled;

}

[Serializable]
public class FlightInfo
{
    public FlightAirport departure;
    public FlightAirport arrival;
    public FlightAirline airline;
    public Flight flight;
    public string flight_date;
    public string flight_status;
}

[Serializable]
public class FlightAirline
{
    public string name;
}

[Serializable]
public class Flight
{
    public string number;
}

public class RandomFlightInfo : MonoBehaviour
{

    [Header("Assign UI Elements: ")]
    [Space]

    [SerializeField] private TextMeshProUGUI destinationText;
    [SerializeField] private TextMeshProUGUI departureTimeText;
    [SerializeField] private TextMeshProUGUI flightNumberText;
    [SerializeField] private TextMeshProUGUI airlineText;
    [SerializeField] private UnityEngine.UI.Button randomFlightButton;

    [Header("The number of flights requested: ")]
    [Space]

    [Tooltip("The obtained flights will be the most recently scheduled ones.")]
    [SerializeField] private string flightCount = "2";

    private const string ApiKey = "32ca66390eea47dbc213bf03cc7c6e09";
    private const string ApiUrl = "http://api.aviationstack.com/v1/flights";

    private FlightData flightData;

    private int currentFlightIndex = 0;

    public string currentRouteDeparture;
    public string currentRouteArrival;

    private void Start()
    {
        randomFlightButton.onClick.AddListener(GetRandomFlight);
        currentRouteDeparture = "";
        currentRouteArrival = ""; 
    }
    private void Update()
    {
        UpdateRoute(currentFlightIndex);
    }
    public void GetRandomFlight()
    {
        string url = $"{ApiUrl}?access_key={ApiKey}&limit={flightCount}";
        StartCoroutine(FetchFlightData(url));
    }

    private void DisplayFlightInfo(FlightInfo[] flights, int index)
    {
        if (flights != null && flights.Length > index)
        {
            FlightInfo flightInfo = flights[index];

            // Display data in the Unity Editor
            string route = $"{flightInfo.departure.iata} to {flightInfo.arrival.iata}";
            string iataCodes = $"{flightInfo.departure.iata} to {flightInfo.arrival.iata}";
            destinationText.text = $"Route: {flightInfo.departure.iata} to {flightInfo.arrival.iata}";
            flightNumberText.text = $"Flight Number: {flightInfo.flight.number}";
            airlineText.text = $"Airline: {flightInfo.airline.name}";
            departureTimeText.text = $"Departure Time: {flightInfo.departure.scheduled}";
            
            // Display IATA codes

            // You can display iataCodes as needed.
        }
    }
    private void UpdateRoute(int index)
    {   

            if(flightData!=null&&flightData.data.Length>0)
            {            
                FlightInfo flightInfo = flightData.data[index];
                currentRouteArrival = flightInfo.departure.iata;
                currentRouteDeparture = flightInfo.arrival.iata;
            }

    }

    private IEnumerator FetchFlightData(string url)
    {
        Debug.Log($"Status: Getting {flightCount} flights...");

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {www.error}");
            }
            else
            {
                // Parse the JSON response
                flightData = JsonUtility.FromJson<FlightData>(www.downloadHandler.text);

                if (flightData != null && flightData.data.Length > 0)
                {
                    Debug.Log($"Received {flightData.data.Length} flights.");
                    DisplayFlightInfo(currentFlightIndex);
                }
                else
                {
                    Debug.LogError("No flight data found in the response.");
                }
            }
        }
    }

    private void DisplayFlightInfo(int index)
    {
        if (flightData != null && flightData.data.Length > index)
        {
            FlightInfo flightInfo = flightData.data[index];

            // Display data in the Unity Editor
            string route = $"{flightInfo.departure.airport} to {flightInfo.arrival.airport}";
            destinationText.text = $"Route: {route}";
            flightNumberText.text = $"Flight Number: {flightInfo.flight.number}";
            airlineText.text = $"Airline: {flightInfo.airline.name}";
            departureTimeText.text = $"Departure Time: {flightInfo.departure.scheduled}";
        }
    }

    public void NextFlight()
    {
        currentFlightIndex = (currentFlightIndex + 1) % flightData.data.Length;
        DisplayFlightInfo(currentFlightIndex);
    }

    public void PreviousFlight()
    {
        currentFlightIndex = (currentFlightIndex - 1 + flightData.data.Length) % flightData.data.Length;
        DisplayFlightInfo(currentFlightIndex);
    }
    public List<string> GetCurrentRoute()
    {   
        List<string> returnedRoute = new List<string>();
        returnedRoute.Add(currentRouteDeparture);
        returnedRoute.Add(currentRouteArrival);
        return returnedRoute;

    }
}
