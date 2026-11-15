using System;
namespace RepairCenter.Models
{
    public class Refrigerator : Appliance
    {
        private readonly bool needsGasRefill;
        private const decimal HourlyRate = 25m;
        private const decimal ExtraServiceCost = 40m;
        public bool NeedsGasRefill { get { return needsGasRefill; } }
        public Refrigerator(string serial, string customer, DateTime received, string fault,
            decimal parts, decimal hours, bool extraService)
            : base(serial, customer, received, fault, parts, hours)
        {
            needsGasRefill = extraService;
        }
        public override decimal CalculateRepairCost()
        {
            return decimal.Round(PartsCost + LaborHours * HourlyRate
                + (needsGasRefill ? ExtraServiceCost : 0m), 2);
        }
    }
}
