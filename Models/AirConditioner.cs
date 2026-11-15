using System;
namespace RepairCenter.Models
{
    public class AirConditioner : Appliance
    {
        private readonly bool needsDeepCleaning;
        private const decimal HourlyRate = 30m;
        private const decimal ExtraServiceCost = 50m;
        public bool NeedsDeepCleaning { get { return needsDeepCleaning; } }
        public AirConditioner(string serial, string customer, DateTime received, string fault,
            decimal parts, decimal hours, bool extraService)
            : base(serial, customer, received, fault, parts, hours)
        {
            needsDeepCleaning = extraService;
        }
        public override decimal CalculateRepairCost()
        {
            return decimal.Round(PartsCost + LaborHours * HourlyRate
                + (needsDeepCleaning ? ExtraServiceCost : 0m), 2);
        }
    }
}
