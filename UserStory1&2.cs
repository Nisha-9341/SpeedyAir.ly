using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SpeedyAirFreight
{
   public class Flight
    {
        public int FlightNumber { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public int Day { get; set; }
        public List<String> Orders { get; set; } = new List<String>();
        public int Capacity { get; } = 20;

        public Flight(int flightNumber, string departure, string arrival, int day)
        {
            FlightNumber = flightNumber;
            Departure = departure;
            Arrival = arrival;
            Day = day;
        }
        public override string ToString()
        {
            return $"Flight: {FlightNumber}, departure: {Departure}, arrival: {Arrival}, day: {Day}";
        }

        public bool AddOrder(string order)
        {
            if (Orders.Count < Capacity)
            {
                Orders.Add(order);
                return true;
            }
            return false;
        }

    }

    public class OrderScheduler
    {
        private List<Flight> flights = new List<Flight>();

        private Dictionary<string, string> orders = new Dictionary<string, string>();
        public void LoadFlights()
        {
            Console.Write("User Story 1: List of the loaded flight schedule.\n");
            Console.WriteLine("Enter the number of flights to schedule:");
            int numberOfFlights = int.Parse(Console.ReadLine());

            for (int i = 1; i <= numberOfFlights; i++)
            {
                Console.WriteLine($"Enter details for flight {i}:");
                Console.Write("Departure (Montreal (YUL)): ");
                string departure = Console.ReadLine();
                Console.Write("Arrival (Toronto (YYZ)/ Calgary (YYC)/ Vancouver (YVR)):\n");
                string arrival = Console.ReadLine();
                Console.Write("Day: ");
                int day = int.Parse(Console.ReadLine());

                flights.Add(new Flight(i, departure, arrival, day));
            }
        }
        public void DisplaySchedule()
        {
            foreach (Flight flight in flights)
            {
                Console.WriteLine(flight);
            }
        }

        public void LoadOrders(string jsonFilePath)
        {
           
             jsonFilePath = "path_to_orders.json";

            // Read the JSON file content
            string jsonData = File.ReadAllText(jsonFilePath);
            var jsonObject = JObject.Parse(jsonData);

            // Iterate through each property in the JSON object
            foreach (var property in jsonObject.Properties())
            {
                // Assuming each property is an object that contains further properties
                var childObject = property.Value as JObject;
                if (childObject != null)
                {
                    foreach (var childProperty in childObject.Properties())
                    {
                        // Use the parent property name as key and child property value as value
                        orders.Add(property.Name, childProperty.Value.ToString());
                    }
                }
            }

        }

        public void ScheduleOrders()
        {
            Console.Write("\nPress Any Key To Continue");
            Console.ReadKey();
            Console.Write("\nUser Story 2: Flight itineraries by scheduling a batch of orders.\n\n");
            foreach (var order in orders)
            {
                bool scheduled = false;
                foreach (Flight flight in flights)
                {
                    if (flight.Arrival == order.Value && flight.AddOrder(order.Key))
                    {
                        Console.WriteLine($"order: {order.Key}, flightNumber: {flight.FlightNumber}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
                        scheduled = true;
                        break;
                    }
                }
                if (!scheduled)
                {
                    Console.WriteLine($"order: {order.Key}, flightNumber: not scheduled");
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            OrderScheduler scheduler = new OrderScheduler();
            scheduler.LoadFlights(); // Load predefined flights
            scheduler.DisplaySchedule(); // Display the flight schedule
            scheduler.LoadOrders("path_to_orders.json"); // Load orders from JSON file
            scheduler.ScheduleOrders(); // Schedule orders and print output
        }
    }
}