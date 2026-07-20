using Haztech.SpriteEditor.Data;
using UnityEditor;
using UnityEngine;
namespace Haztech.SpriteEditor.Editor
{
    public static class LayerRow
    {
        private static int draggedId = -1;
        public static void Draw(ToolWindow window, int index)
        {
            SpriteConfig config = window.SpriteConfig;
            if (config == null) return;
            LayerObject layerObj = config.ExpandedLayers[index];
            if (layerObj == null) return;

            const float padding = 2f;
            const float buttonSize = 18f;
            Rect rowRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            bool selected = config.selectedLayer == index;

            Rect eyeRect = new Rect(
                rowRect.x + padding,
                rowRect.y,
                buttonSize,
                rowRect.height);

            Rect downRect = new Rect(
                rowRect.xMax - buttonSize - padding,
                rowRect.y,
                buttonSize,
                rowRect.height);

            Rect upRect = new Rect(
                downRect.x - buttonSize - padding,
                rowRect.y,
                buttonSize,
                rowRect.height);

            Rect labelRect = new Rect(
                eyeRect.xMax + 4f,
                rowRect.y,
                rowRect.width - eyeRect.xMax - 8f,
                rowRect.height);

            bool mouseOverButton =
                eyeRect.Contains(Event.current.mousePosition) ||
                upRect.Contains(Event.current.mousePosition) ||
                downRect.Contains(Event.current.mousePosition);

            if (!mouseOverButton && Event.current.type == EventType.MouseDown &&
                rowRect.Contains(Event.current.mousePosition))
            {
                config.selectedLayer = index;
                draggedId = index;
                Event.current.Use();
                window.Repaint();
            }

            bool grouping = false;

            if (draggedId >= 0 && Event.current.type == EventType.MouseDrag)
            {
                if (index != draggedId && rowRect.Contains(Event.current.mousePosition))
                {
                    if (layerObj is Layer)
                    {
                        //config.MoveLayer(draggedLayer, index);

                        //draggedLayer = index;
                        //config.selectedLayer = index;

                        //EditorUtility.SetDirty(config);
                    }
                    else if (layerObj is LayerGroup)
                    {
                        grouping = true;
                    }

                    window.Repaint();

                    Event.current.Use();
                }


            }


            if (draggedId >= 0 &&
                Event.current.type == EventType.MouseUp &&
                rowRect.Contains(Event.current.mousePosition))
            {
                if (index != draggedId)
                {
                    if (layerObj is LayerGroup group)
                    {
                        Layer draggedLayer = config.GetLayer(draggedId);

                        config.AddLayerToGroup(draggedLayer, group);

                        EditorUtility.SetDirty(config);
                    }
                }

                draggedId = -1;
                Event.current.Use();
                window.Repaint();
            }

            // Draw layer rect
            if (grouping)
            {
                EditorGUI.DrawRect(
                    rowRect,
                    new Color(0.24f, 0.48f, 1f));
            }
            else if (selected)
            {
                EditorGUI.DrawRect(
                    rowRect,
                    new Color(0.24f, 0.48f, 0.85f));
            }
            else if (rowRect.Contains(Event.current.mousePosition))
            {
                EditorGUI.DrawRect(
                    rowRect,
                    new Color(1f, 1f, 1f, 0.08f));
            }

            GUIContent eyeIcon = EditorGUIUtility.IconContent(
                                    layerObj.visible
                                        ? "animationvisibilitytoggleon"
                                        : "animationvisibilitytoggleoff");

            if (GUI.Button(eyeRect, eyeIcon, EditorStyles.iconButton))
            {
                Undo.RecordObject(config, "Toggle Layer Visibility");

                layerObj.visible = !layerObj.visible;

                EditorUtility.SetDirty(config);
                window.Repaint();
            }

            Layer layer = null;
            if (layerObj is Layer) layer = (Layer)layerObj;



            if (layerObj != null && index > 0 && 
                GUI.Button(upRect, EditorGUIUtility.IconContent("scrollup"), EditorStyles.iconButton))
            {
                window.SpriteConfig.MoveLayerUp(index);
                window.SpriteConfig.selectedLayer--;
                EditorUtility.SetDirty(window.SpriteConfig);
                window.Repaint();
            }

            if (layerObj != null && index < config.ExpandedLayers.Count -1 && 
                GUI.Button(downRect, EditorGUIUtility.IconContent("scrolldown"), EditorStyles.iconButton))
            {
                window.SpriteConfig.MoveLayerDown(index);
                window.SpriteConfig.selectedLayer++;
                EditorUtility.SetDirty(window.SpriteConfig);
                window.Repaint();
            }

            string label = layerObj.name;

            if (layer != null && layer.InGroup)
            {
                label = "   " + label;
            }

            GUI.Label(
                new Rect(
                    labelRect.x + 4f,
                    labelRect.y,
                    labelRect.width - 8f,
                    labelRect.height),
                    label);
        }
    }
}
