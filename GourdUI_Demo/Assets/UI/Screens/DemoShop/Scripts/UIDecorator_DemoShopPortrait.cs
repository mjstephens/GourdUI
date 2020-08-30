using UnityEngine;

public class UIDecorator_DemoShopPortrait : MonoBehaviour
{
    #region Variables

    [Header("Values")]
    public float selectedIconMoveSpeed = 10;
    
    [Header("References")]
    public RectTransform categorySelectionMover;
    public RectTransform category1SelectionTarget;
    public RectTransform category2SelectionTarget;
    public RectTransform category3SelectionTarget;

    private RectTransform _currentTarget;

    #endregion Variables
    

    #region Selection

    public void OnCategory1Selected()
    {
        _currentTarget = category1SelectionTarget;
    }
    
    public void OnCategory2Selected()
    {
        _currentTarget = category2SelectionTarget;
    }
    
    public void OnCategory3Selected()
    {
        _currentTarget = category3SelectionTarget;
    }
    
    #endregion Selection


    #region Update

    private void Update()
    {
        if (_currentTarget == null)
            return;
        
        categorySelectionMover.transform.position = Vector3.Lerp(
            categorySelectionMover.transform.position, 
            _currentTarget.position,
            Time.deltaTime * selectedIconMoveSpeed);
    }

    #endregion Update
}
