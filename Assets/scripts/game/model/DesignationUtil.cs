﻿using game.view.system;
using game.view.system.mouse_tool;
using util.lang;

namespace game.model {
    public class DesignationUtil : Singleton<DesignationUtil> {

        public static void applyTool(MouseToolEnum tool, int x, int y, int z) {
            get()._applyTool(tool, x, y, z);
        }
        
        public void _applyTool(MouseToolEnum tool, int x, int y, int z) {
            if(validateDesignation(tool, x, y, z)) createDesignation(tool, x, y, z);
        }
        
        private bool validateDesignation(MouseToolEnum tool, int x, int y, int z) {
            
            return true;
            // return false;
        }
        
        private void createDesignation(MouseToolEnum tool, int x, int y, int z) {
            
        }
    }
}