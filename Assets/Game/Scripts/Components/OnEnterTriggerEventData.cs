namespace Game.Components
{
    public struct OnEnterTriggerEventData
    {
        public int TriggerEntity;
        public int ColliderEntity;
    }
    
    public struct OnStayTriggerEventData
    {
        public int TriggerEntity;
        public int ColliderEntity;
    }
    
    public struct OnExitTriggerEventData
    {
        public int TriggerEntity;
        public int ColliderEntity;
    }
}