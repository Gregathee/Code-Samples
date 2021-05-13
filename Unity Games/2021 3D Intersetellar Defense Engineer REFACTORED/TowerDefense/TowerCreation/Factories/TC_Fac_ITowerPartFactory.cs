namespace TowerDefense.TowerCreation.Factories
{
    /// <summary>
    /// Tower Part Factory Interface
    /// </summary>
    public interface TC_Fac_ITowerPartFactory<T>
    {
        T CreateTowerPart();
    }
}
