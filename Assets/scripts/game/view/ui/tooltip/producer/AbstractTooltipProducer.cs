using System.Linq;
using game.view.ui.tooltip.handler;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game.view.ui.tooltip.producer {
public abstract class AbstractTooltipProducer : MonoBehaviour {
    protected Canvas tooltipCanvas;

    public void Awake() {
        tooltipCanvas = SceneManager.GetActiveScene()
            .GetRootGameObjects().First(go => go.name.Equals("TooltipCanvas")).GetComponent<Canvas>();
    }
    
    public abstract AbstractTooltipHandler openTooltip(Vector3 position);
    
    public abstract void closeTooltip();
}
}