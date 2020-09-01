namespace GourdUI
{
    public interface IUIStateUpdateReceivable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateData"></param>
        /// <typeparam name="T"></typeparam>
        void OnStateDataUpdated<T>(T stateData) where T : UIState;
    }
}