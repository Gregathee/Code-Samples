using UnityEngine;
namespace TowerDefense.TowerParts
{
    /// <summary>
    /// Base class for tower parts with customizable color regions. 
    /// </summary>
    public abstract class ColoredTowerPart : TowerPart
    {
        public Material Mat1;
        public Material Mat2;
        public Material Mat3;
        
        [SerializeField] MeshRenderer[] _colorSection1;
        [SerializeField] MeshRenderer[] _colorSection2;
        [SerializeField] MeshRenderer[] _colorSection3;

        bool _initialized;

        protected virtual void Start() { Initialize(); }

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = false;
            Mat1 = new Material(Mat1);
            Mat2 = new Material(Mat2);
            Mat3 = new Material(Mat3);
        }
        
        public void SetMaterials(Material m1, Material m2, Material m3)
        {
            Mat1 = m1;
            Mat2 = m2;
            Mat3 = m3;
            foreach (MeshRenderer mRenderer in _colorSection1) mRenderer.material = Mat1;
            foreach (MeshRenderer mRenderer in _colorSection2) mRenderer.material = Mat2;
            foreach (MeshRenderer mRenderer in _colorSection3) mRenderer.material = Mat3;
        }
    }
}
