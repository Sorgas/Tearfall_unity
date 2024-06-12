using game.model;
using game.model.localmap;
using TMPro;
using types.material;
using UnityEngine;

namespace game.view.ui {
public class DebugInfoPanelHandler : MonoBehaviour {
    public TextMeshProUGUI text;
    
    private void Update() {
        if (GameView.get().cameraAndMouseHandler == null) return;
        Vector3Int modelPosition = GameView.get().cameraAndMouseHandler.selector.position;
        LocalMap map = GameModel.get().currentLocalModel.localMap;
        if (!map.inMap(modelPosition)) return;
        text.text =
            $"pos: [{modelPosition.x},  {modelPosition.y},  {modelPosition.z}]\n" +
            $"block: {map.blockType.getEnumValue(modelPosition).NAME} {MaterialMap.get().material(map.blockType.getMaterial(modelPosition)).name}\n" +
            $"passage: {map.passageMap.getPassageType(modelPosition).name}\n" +
            $"area: {map.passageMap.defaultHelper.getArea(modelPosition)}\n" +
            $"area(rooms): {map.passageMap.groundNoDoorsHelper.getArea(modelPosition)}\n" + 
            $"area(fly): {map.passageMap.indoorHelper.getArea(modelPosition)}\n" + 
            $"UPS: {GameModel.get().counter.lastUps}"; 
    }
}
}