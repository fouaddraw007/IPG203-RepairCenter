namespace RepairCenter.Models
{
    public enum RepairStatus { Received, UnderRepair, Repaired }
    public delegate void StatusChangedHandler(Appliance appliance, RepairStatus previousStatus);
}
