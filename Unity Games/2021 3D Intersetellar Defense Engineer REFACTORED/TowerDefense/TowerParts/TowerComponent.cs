using UnityEngine;
namespace TowerDefense.TowerParts
{
    /// <summary>
    /// Base class for any object that pertains to a towers functionality.
    /// </summary>
    public abstract class TowerComponent : MonoBehaviour
    {
        public string CustomPartFilePath = "";
        protected bool IsPreview = true;
        [SerializeField] GameObject[] _rotatableParts;
        [SerializeField] protected Camera _view;
        //[SerializeField] protected List<Light> showcaseLights = new List<Light>();
        public bool Shrink = true;
        public bool Hide = true;

        const float _rotatePartSpeed = 50;
        
        delegate void TP_Behavior();
        TP_Behavior _tpBehavior;
        
        protected virtual void Awake()
        {
            if (!_view) { _view = GetComponentInChildren<Camera>(); }
            if (_view){ _view.targetTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);}
            _tpBehavior = PreviewBehavior;
        }

        protected virtual void Update() { _tpBehavior(); }
        
        public void SetRotatableParts(GameObject[] newParts) { _rotatableParts = newParts;}

        public void CorrectRotation() { foreach (GameObject g in _rotatableParts) { g.transform.localEulerAngles = Vector3.zero; } }

        public virtual void SetShrink(bool shrinkIn) { Shrink = shrinkIn; }
        public virtual void SetHide(bool hideIn) { Hide = hideIn; }
        public virtual void SetIsPreview(bool preview) { _tpBehavior = (IsPreview = preview) ? (TP_Behavior) PreviewBehavior : DoNothing; }

        public void ActivateCamera(bool activate) { _view.gameObject.SetActive(activate); }

        public ref Camera GetView() { return ref _view; }
        public void SetView(Camera newView)
        {
            _view = newView;
            _view.targetTexture = new RenderTexture(_view.targetTexture);
        }
        
        void PreviewBehavior()
        {
            if (_rotatableParts == null) return ;
            foreach (GameObject gameOb in _rotatableParts)
            {
                if (!gameOb) continue;
                float newAngle = gameOb.transform.localEulerAngles.y + (_rotatePartSpeed * Time.deltaTime);
                gameOb.transform.localEulerAngles = new Vector3(0, newAngle, 0);
            }
        }
        void DoNothing(){}
    }
}
