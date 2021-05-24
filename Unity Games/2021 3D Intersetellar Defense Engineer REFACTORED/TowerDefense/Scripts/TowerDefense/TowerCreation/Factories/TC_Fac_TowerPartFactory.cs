using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;
using UnityEngine;
namespace TowerDefense.TowerCreation.Factories
{
    /// <summary>
    /// Tower Part Factory base class
    /// </summary>
    public abstract class TC_Fac_TowerPartFactory : MonoBehaviour
    {
        public TC_UI_TP_Inventory Inventory;
        [SerializeField] TC_ColorEditor _colorEditor;
        
        void OnEnable() { SetAsActiveFactory(); }
        
        // Constructs and returns a tower component from the tower editor.
        public abstract TowerComponent CreateTowerPart(string partName);
        
        // Deconstructs a tower component and displays its properties in the tower editor.
        public abstract void DisplayPartProperties(TowerComponent part);

        /// <summary>
        /// Returns the current tower component model to have its colors modified. 
        /// </summary>
        /// <returns></returns>
        public abstract ColoredTowerPart GetColoredTowerPart1();
        /// <summary>
        /// Returns the current tower component model to have its colors modified. 
        /// </summary>
        /// <returns></returns>
        public abstract ColoredTowerPart GetColoredTowerPart2();

        public virtual void SetAsActiveFactory()
        {
            TC_Fac_TowerPart.ActiveFactory = this;
            
            // Notify the color editor to change display colors for tower components.
            ColoredTowerPart part = GetColoredTowerPart1();
            _colorEditor.SetIcon1Colors(part.Mat1.color, part.Mat2.color, part.Mat3.color);
            part = GetColoredTowerPart2();
            if (part == null) return;
            _colorEditor.SetIcon2Colors(part.Mat1.color, part.Mat2.color, part.Mat3.color);
        }

        public virtual bool ErrorsPresent() { return false; }
    }
}
