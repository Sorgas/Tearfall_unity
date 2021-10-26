using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.jobs_widget {
    public class UnitJobRow : MonoBehaviour {
        [SerializeField]
        public EcsEntity unit;
        [SerializeReference]
        public Dictionary<string, Toggle> toggles = new Dictionary<string, Toggle>();
    }
}