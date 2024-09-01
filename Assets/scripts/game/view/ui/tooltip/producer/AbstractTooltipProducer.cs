using System.Linq;
using game.view.ui.tooltip.handler;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game.view.ui.tooltip.producer {
public abstract class AbstractTooltipProducer : MonoBehaviour {
    protected Canvas tooltipCanvas;
    protected InfoTooltipData data;
    
    public void Awake() {
        tooltipCanvas = SceneManager.GetActiveScene()
            .GetRootGameObjects().First(go => go.name.Equals("TooltipCanvas")).GetComponent<Canvas>();
    }
    
    public abstract AbstractTooltipHandler openTooltip(Vector3 position);
    
    public abstract void closeTooltip();

    public void setData(InfoTooltipData data) {
        this.data = data;
    }
}
}