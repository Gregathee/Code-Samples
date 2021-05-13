using System.Collections.Generic;
namespace TowerDefense.TowerParts
{
    /// <summary>
    /// Interface that enables tower parts to serialize themselves. 
    /// </summary>
    public interface ISerializableTowerComponent
    {
        void SaveToFile();

        void SetPropertiesFromJSON(Dictionary<string, string> jsonDict);
    }
}
