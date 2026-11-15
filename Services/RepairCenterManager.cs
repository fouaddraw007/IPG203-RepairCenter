using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RepairCenter.Models;
namespace RepairCenter.Services
{
    public class RepairCenterManager
    {
        private readonly List<Appliance> appliances = new List<Appliance>();
        private static int registeredDeviceCount;
        // Counts accepted registrations across all managers during this program run.
        public static int RegisteredDeviceCount { get { return registeredDeviceCount; } }
        public ReadOnlyCollection<Appliance> Appliances { get { return appliances.AsReadOnly(); } }
        public void Add(Appliance appliance)
        {
            if (appliance == null) throw new ArgumentNullException("appliance");
            if (Find(appliance.SerialNumber) != null)
                throw new ArgumentException("Serial number already exists in this center.");
            appliances.Add(appliance);
            registeredDeviceCount++;
        }
        public Appliance Find(string serial)
        {
            serial = InputValidator.SerialNumber(serial);
            foreach (Appliance appliance in appliances)
                if (appliance.SerialNumber == serial) return appliance;
            return null;
        }
        public List<Appliance> GetByStatus(RepairStatus status)
        {
            List<Appliance> result = new List<Appliance>();
            foreach (Appliance appliance in appliances)
                if (appliance.Status == status) result.Add(appliance);
            return result;
        }
        public decimal CalculateTotalCost()
        {
            decimal total = 0;
            foreach (Appliance appliance in appliances)
                total += appliance.CalculateRepairCost();
            return total;
        }
    }
}
