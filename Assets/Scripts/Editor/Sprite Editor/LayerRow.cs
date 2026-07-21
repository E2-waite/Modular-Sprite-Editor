using Haztech.SpriteEditor.Data;
using UnityEditor;
using UnityEngine;
namespace Haztech.SpriteEditor.Editor
{


    public static class LayerRow
    {
        const float buttonSize = 18f;

        private class RowRects
        {
            private const float padding = 2f;
            public Rect row;
            public Rect visibility;
            public Rect moveDown;
            public Rect moveUp;
            public Rect label;
            public RowRects(Rect row)
            {
                this.row = row;

                Rect eyeRect = new Rect(
                     row.x + padding,
                     row.y,
                     buttonSize,
                     row.height);

                Rect downRect = new Rect(
                    row.xMax - buttonSize - padding,
                    row.y,
                    buttonSize,
                    row.height);

                Rect upRect = new Rect(
                    downRect.x - buttonSize - padding,
                    row.y,
                    buttonSize,
                    row.height);

                Rect labelRect = new Rect(
                    eyeRect.xMax + 4f,
                    row.y,
                    row.width - eyeRect.xMax - 8f,
                    row.height);
            }

            public bool MouseOverButton =>
                visibility.Contains(Event.current.mousePosition) ||
                moveUp.Contains(Event.current.mousePosition) ||
                moveDown.Contains(Event.current.mousePosition);
            
        }

        private static int draggedId = -1;
        public static void Draw(ToolWindow window, int index)
        {
            SpriteConfig config = window.SpriteConfig;
            if (config == null) return;
            LayerObject layerObj = config.ExpandedLayers[index];
            if (layerObj == null) return;

            bool selected = config.selectedLayer == index;
            RowRects rects = new RowRects(EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight));

            if (!rects.MouseOverButton && Event.current.type == EventType.MouseDown &&
                rects.row.Contains(Event.current.mousePosition))
            {
                config.selectedLayer = index;
                draggedId = index;
                Event.current.Use();
                window.Repaint();
            }

            bool grouping = false;

            if (draggedId >= 0 && Event.current.type == EventType.MouseDrag)
            {
                if (index != draggedId && rects.row.Contains(Event.current.mousePosition))
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
                rects.row.Contains(Event.current.mousePosition))
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
                    rects.row,
                    new Color(0.24f, 0.48f, 1f));
            }
            else if (selected)
            {
                EditorGUI.DrawRect(
                    rects.row,
                    new Color(0.24f, 0.48f, 0.85f));
            }
            else if (rects.row.Contains(Event.current.mousePosition))
            {
                EditorGUI.DrawRect(
                    rects.row,
                    new Color(1f, 1f, 1f, 0.08f));
            }

            GUIContent eyeIcon = EditorGUIUtility.IconContent(
                                    layerObj.visible
                                        ? "animationvisibilitytoggleon"
                                        : "animationvisibilitytoggleoff");

            if (GUI.Button(rects.visibility, eyeIcon, EditorStyles.iconButton))
            {
                Undo.RecordObject(config, "Toggle Layer Visibility");

                layerObj.visible = !layerObj.visible;

                EditorUtility.SetDirty(config);
                window.Repaint();
            }

            Layer layer = null;
            if (layerObj is Layer) layer = (Layer)layerObj;



            if (layerObj != null && index > 0 && 
                GUI.Button(rects.moveUp, EditorGUIUtility.IconContent("scrollup"), EditorStyles.iconButton))
            {
                window.SpriteConfig.MoveLayerUp(index);
                window.SpriteConfig.selectedLayer--;
                EditorUtility.SetDirty(window.SpriteConfig);
                window.Repaint();
            }

            if (layerObj != null && index < config.ExpandedLayers.Count -1 && 
                GUI.Button(rects.moveDown, EditorGUIUtility.IconContent("scrolldown"), EditorStyles.iconButton))
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
                    rects.label.x + 4f,
                    rects.label.y,
                    rects.label.width - 8f,
                    rects.label.height),
                    label);
        }
    }
}
