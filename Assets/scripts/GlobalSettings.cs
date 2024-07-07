public static class GlobalSettings {
    public const int UPDATES_PER_SECOND = 30; // not available to player
    
    public const bool USE_SPRITE_SORTING_LAYERS = false;
    public const int cameraLayerDepth = 8;
    
    // tooltips
    public const float tooltipShowDelay = 0.15f;
    public const float tooltipHideDelay = 0.1f;
    public const float tooltipLockDelay = 0.1f;
    
    // gameplay
    public static int jobAssignPolicy = 0; // 0 enable all, 1 enable with skills, 2 enable hauling, 3 disable all
}