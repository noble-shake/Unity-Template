namespace RottenNoble.Cores.Enum
{
    public enum ApplicationType
    {
        Test = 0,
        Dev = 1,
        Release = 2,
    }

    public enum ResourceEnum
    {
        Resource,
        Local,
        Addressables,
    }

    public enum CanvasType
    {
        Background,
        Hud,
        Popup,
        Overlay,
    }

    public enum VisibleState
    {
        None,
        Appearing,
        Appeared,
        Disappearing,
        Disappeared,
    }

    public enum SoundType
    {
        BGM,
        SFX,
    }

    public enum BusyType
    {
        Dark,
        Light,
    }
}
