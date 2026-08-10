namespace EpicosityPort.Systems.Retrospective
{
    public struct ItemPortStatusEntry
    {
        public PortStatusID PortStatus;

        public ItemPortStatusEntry(PortStatusID portStatus)
        {
            PortStatus = portStatus;
        }
    }
}
