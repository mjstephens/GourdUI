namespace GourdUI
{
    public interface IBaseUIElement
    {
        /// <summary>
        /// When the element is first instantiated
        /// </summary>
        void Setup();
        
        /// <summary>
        /// When the element is destroyed
        /// </summary>
        void Cleanup();
        
        /// <summary>
        /// When the element is activated/made visible
        /// </summary>
        void Show();
        
        /// <summary>
        /// When the element is hidden
        /// </summary>
        void Hide();
    }
}