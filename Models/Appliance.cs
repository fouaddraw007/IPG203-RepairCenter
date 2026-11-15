using System;
using RepairCenter.Services;
namespace RepairCenter.Models
{
    public abstract class Appliance : IRepairable
    {
        private readonly string serialNumber;
        private readonly string customerName;
        private readonly DateTime receivedDate;
        private readonly string faultDescription;
        private readonly decimal partsCost;
        private readonly decimal laborHours;
        private RepairStatus status;

        public string SerialNumber { get { return serialNumber; } }
        public string CustomerName { get { return customerName; } }
        public DateTime ReceivedDate { get { return receivedDate; } }
        public string FaultDescription { get { return faultDescription; } }
        public decimal PartsCost { get { return partsCost; } }
        public decimal LaborHours { get { return laborHours; } }
        public RepairStatus Status { get { return status; } }
        public event StatusChangedHandler StatusChanged;

        protected Appliance(string serial, string customer, DateTime received, string fault,
            decimal parts, decimal hours)
        {
            serialNumber = InputValidator.SerialNumber(serial);
            customerName = InputValidator.RequiredText(customer, "Customer name");
            receivedDate = InputValidator.ReceivedDate(received);
            faultDescription = InputValidator.RequiredText(fault, "Fault description");
            partsCost = InputValidator.NonNegative(parts, "Parts cost");
            laborHours = InputValidator.NonNegative(hours, "Labor hours");
            status = RepairStatus.Received;
        }
        public abstract decimal CalculateRepairCost();
        public void StartRepair()
        {
            if (status != RepairStatus.Received)
                throw new InvalidOperationException("Only received devices can start repair.");
            ChangeStatus(RepairStatus.UnderRepair);
        }
        public void CompleteRepair()
        {
            if (status != RepairStatus.UnderRepair)
                throw new InvalidOperationException("Start repair before completing it.");
            ChangeStatus(RepairStatus.Repaired);
        }
        private void ChangeStatus(RepairStatus next)
        {
            RepairStatus previous = status;
            status = next;
            StatusChangedHandler handler = StatusChanged;
            if (handler != null) handler(this, previous);
        }
    }
}
