using System;
using System.Collections.Generic;
using System.Globalization;
using RepairCenter.Models;
using RepairCenter.Services;
namespace RepairCenter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            RepairCenterManager center = new RepairCenterManager();
            if (args.Length > 0 && args[0] == "--demo") { Demo(center); return; }
            Console.WriteLine("Home Appliance Repair Center - IPG203");
            while (true)
            {
                Console.WriteLine("\n1 Add | 2 List all | 3 Start repair | 4 Complete repair");
                Console.WriteLine("5 Under repair | 6 Repaired | 7 Totals | 0 Exit");
                string choice = Console.ReadLine();
                if (choice == null || choice == "0") return;
                try
                {
                    switch (choice)
                    {
                        case "1": AddDevice(center); break;
                        case "2": Display(center.Appliances); break;
                        case "3": SelectDevice(center).StartRepair(); break;
                        case "4": SelectDevice(center).CompleteRepair(); break;
                        case "5": Display(center.GetByStatus(RepairStatus.UnderRepair)); break;
                        case "6": Display(center.GetByStatus(RepairStatus.Repaired)); break;
                        case "7": Totals(center); break;
                        default: Console.WriteLine("Choose a menu number."); break;
                    }
                }
                catch (ArgumentException ex) { Console.WriteLine("Input error: " + ex.Message); }
                catch (InvalidOperationException ex) { Console.WriteLine(ex.Message); }
            }
        }
        private static string Read(string prompt)
        {
            Console.Write(prompt + ": ");
            string value = Console.ReadLine();
            if (value == null) throw new InvalidOperationException("Input closed.");
            return InputValidator.RequiredText(value, prompt);
        }
        private static decimal ReadNumber(string prompt)
        {
            decimal result;
            if (!decimal.TryParse(Read(prompt), NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out result))
                throw new ArgumentException("Enter a non-negative number using a decimal point.");
            return InputValidator.NonNegative(result, prompt);
        }
        private static void AddDevice(RepairCenterManager center)
        {
            string type = Read("Type: 1 Refrigerator, 2 Washing machine, 3 Air conditioner");
            if (type != "1" && type != "2" && type != "3") throw new ArgumentException("Unknown device type.");
            string serial = Read("Serial number");
            string customer = Read("Customer name");
            string fault = Read("Fault description");
            DateTime date;
            if (!DateTime.TryParseExact(Read("Received date (yyyy-MM-dd)"), "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                throw new ArgumentException("Invalid date format.");
            decimal parts = ReadNumber("Parts cost");
            decimal hours = ReadNumber("Labor hours");
            string service = type == "1" ? "Gas refill" : type == "2" ? "Drum service" : "Deep cleaning";
            string answer = Read(service + " required? y/n").ToLowerInvariant();
            if (answer != "y" && answer != "n") throw new ArgumentException("Enter y or n.");
            bool extra = answer == "y";
            Appliance device;
            if (type == "1") device = new Refrigerator(serial, customer, date, fault, parts, hours, extra);
            else if (type == "2") device = new WashingMachine(serial, customer, date, fault, parts, hours, extra);
            else device = new AirConditioner(serial, customer, date, fault, parts, hours, extra);
            Register(center, device);
            Console.WriteLine("Device added. Estimated cost: " + device.CalculateRepairCost().ToString("F2"));
        }
        private static void Register(RepairCenterManager center, Appliance device)
        {
            center.Add(device);
            device.StatusChanged += OnStatusChanged;
        }
        private static Appliance SelectDevice(RepairCenterManager center)
        {
            Appliance device = center.Find(Read("Serial number"));
            if (device == null) throw new ArgumentException("Device not found.");
            return device;
        }
        private static void OnStatusChanged(Appliance device, RepairStatus previous)
        {
            Console.WriteLine("Status: " + device.SerialNumber + " | " + previous + " -> " + device.Status);
            if (device.Status == RepairStatus.Repaired)
                Console.WriteLine("Repair completed. Ready for collection. Cost: " + device.CalculateRepairCost().ToString("F2"));
        }
        private static void Display(IEnumerable<Appliance> devices)
        {
            bool any = false;
            foreach (Appliance device in devices)
            {
                any = true;
                Console.WriteLine("{0} | {1} | {2} | {3} | {4:F2}", device.SerialNumber,
                    device.GetType().Name, device.CustomerName, device.Status, device.CalculateRepairCost());
                Console.WriteLine("  Received: {0:yyyy-MM-dd}; Fault: {1}", device.ReceivedDate, device.FaultDescription);
            }
            if (!any) Console.WriteLine("No devices found.");
        }
        private static void Totals(RepairCenterManager center)
        {
            Console.WriteLine("Registered this run: " + RepairCenterManager.RegisteredDeviceCount);
            Console.WriteLine("Received: " + center.GetByStatus(RepairStatus.Received).Count);
            Console.WriteLine("Under repair: " + center.GetByStatus(RepairStatus.UnderRepair).Count);
            Console.WriteLine("Repaired: " + center.GetByStatus(RepairStatus.Repaired).Count);
            Console.WriteLine("Total estimated cost (all devices): " + center.CalculateTotalCost().ToString("F2"));
        }
        private static void Demo(RepairCenterManager center)
        {
            Register(center, new Refrigerator("REF-001", "Ahmad", DateTime.Today, "Weak cooling", 100m, 2m, true));
            Register(center, new WashingMachine("WM-001", "Sara", DateTime.Today, "Drum noise", 80m, 1.5m, true));
            Register(center, new AirConditioner("AC-001", "Omar", DateTime.Today, "Low airflow", 60m, 2m, true));
            center.Find("REF-001").StartRepair();
            center.Find("REF-001").CompleteRepair();
            center.Find("WM-001").StartRepair();
            Display(center.Appliances);
            Totals(center);
        }
    }
}
