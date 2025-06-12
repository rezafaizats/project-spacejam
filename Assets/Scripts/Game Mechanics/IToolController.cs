namespace Game_Mechanics
{
    public interface IToolController
    {
        public string ToolId { get; }
        public void SetIsEquipped(bool isEquipped);
    }
} 