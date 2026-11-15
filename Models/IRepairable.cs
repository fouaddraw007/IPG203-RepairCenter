namespace RepairCenter.Models
{
    public interface IRepairable
    {
        decimal CalculateRepairCost();
        void StartRepair();
        void CompleteRepair();
    }
}
