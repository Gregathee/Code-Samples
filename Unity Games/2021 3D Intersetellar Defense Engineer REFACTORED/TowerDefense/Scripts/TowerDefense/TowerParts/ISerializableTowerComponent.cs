using System.Collections.Generic;
using TowerDefense.TowerCreation.Factories;
using TowerDefense.TowerCreation.UI.Inventory;
namespace TowerDefense.TowerParts
{
    /// <summary>
    /// Interface that enables tower parts to serialize themselves. 
    /// </summary>
    public interface ISerializableTowerComponent
    {
        void SaveToFile();

        void SetPropertiesFromJSON(Dictionary<string, string> jsonDict);

        void DeleteFile(bool forceDelete);

        void UpdateDependencies(TowerComponent oldPart);

        TC_UI_TP_Inventory GetInventory();

        TC_Fac_TowerPartFactory GetFactory();

        void GenerateFileName();
    }
}
