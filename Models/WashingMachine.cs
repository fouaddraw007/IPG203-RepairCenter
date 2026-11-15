using System;
namespace RepairCenter.Models
{
    public class WashingMachine : Appliance
    {
        private readonly bool needsDrumService;
        private const decimal HourlyRate = 20m;
        private const decimal ExtraServiceCost = 35m;
        public bool NeedsDrumService { get { return needsDrumService; } }
        public WashingMachine(string serial, string customer, DateTime received, string fault,
            decimal parts, decimal hours, bool extraService)
            : base(serial, customer, received, fault, parts, hours)
        {
            needsDrumService = extraService;
        }
        public override decimal CalculateRepairCost()
        {
            return decimal.Round(PartsCost + LaborHours * HourlyRate
                + (needsDrumService ? ExtraServiceCost : 0m), 2);
        }
    }
}
