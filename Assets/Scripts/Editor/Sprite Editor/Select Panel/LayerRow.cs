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
            public Rect expand;

            public RowRects(Rect row, bool isGroup, bool inGroup)
            {
                this.row = row;

                float currentX = row.x + padding;

                visibility = new Rect(
                    currentX,
                    row.y,
                    buttonSize,
                    row.height);
                currentX = visibility.xMax + padding;

                if (isGroup || inGroup)
                {
                    expand = new Rect(
                        currentX,
                        row.y,
                        buttonSize,
                        row.height);

                    currentX = expand.xMax;
                }
                else
                {
                    expand = Rect.zero;
                }

                moveDown = new Rect(
                    row.xMax - buttonSize - padding,
                    row.y,
                    buttonSize,
                    row.height);

                moveUp = new Rect(
                    moveDown.x - buttonSize - padding,
                    row.y,
                    buttonSize,
                    row.height);

                label = new Rect(
                    (isGroup || inGroup ? expand.xMax : visibility.xMax),
                    row.y,
                    moveUp.x - (isGroup || inGroup ? expand.xMax : 0) - visibility.xMax - 8f,
                    row.height);
            }

            public bool MouseOverButton(Vector2 mousePos) =>
                visibility.Contains(mousePos) ||
                moveUp.Contains(mousePos) ||
                moveDown.Contains(mousePos) || 
                expand.Contains(mousePos);

            public bool MouseOverRow(Vector2 mousePos) =>
                !MouseOverButton(mousePos) && row.Contains(mousePos);

        }

        private static int draggedId = -1;
        private static int groupId = -1;
        private static Window toolWindow;
        private static SpriteConfig config;

        public static void Draw(int index)
        {
            if (Window.Instance == null) return;
            toolWindow = Window.Instance;
            config = Window.Instance.SpriteConfig;
            if (config == null) return;
            LayerObject layerObj = config.ExpandedLayers[index];
            if (layerObj == null) return;

            Layer layer = null;
            LayerGroup group = null;

            if (layerObj is Layer)
                layer = (Layer)layerObj;
            else if (layerObj is LayerGroup)
                group = (LayerGroup)layerObj;

            bool selected = config.selectedLayer == index;
            bool isGroup = group != null;
            bool inGroup = layer != null && layer.InGroup;

            RowRects rects = new RowRects(EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight), isGroup, inGroup);

            bool grouping = false;

            HandleMouse(index, rects, layerObj, ref grouping);

            // Draw row rect
            if (index == groupId)
            {
                EditorGUI.DrawRect(
                    rects.row,
                    new Color(1f, 1f, 1f, 1f));
            }
            else if (selected)
            {
                EditorGUI.DrawRect(
                    rects.row,
                    new Color(0.24f, 0.48f, 0.85f));
            }
            else if (isGroup)
            {
                EditorGUI.DrawRect(
                    rects.row,
                    new Color(1f, 1f, 1f, .05f));
            }

            DrawButtons(index, rects, layerObj);

            // Draw label
            string label = layerObj.name;
            GUI.Label(
                new Rect(
                    rects.label.x + 4f,
                    rects.label.y,
                    rects.label.width - 8f,
                    rects.label.height),
                label);
        }

        private static void HandleMouse(int index, RowRects rects, LayerObject layerObj, ref bool grouping)
        {
            if (Event.current.type == EventType.MouseDown && rects.MouseOverRow(Event.current.mousePosition))
            {
                config.selectedLayer = index;
                draggedId = index;
                Event.current.Use();
                toolWindow.Repaint();
            }

            if (draggedId >= 0 && Event.current.type == EventType.MouseDrag)
            {
                if (rects.row.Contains(Event.current.mousePosition))
                {
                    if (layerObj is Layer layer)
                    {
                        if (layer.InGroup)
                            groupId = layer.Group.id;
                        else
                            groupId = -1;
                        //config.MoveLayer(draggedLayer, index);

                        //draggedLayer = index;
                        //config.selectedLayer = index;

                        //EditorUtility.SetDirty(config);
                    }
                    else if (layerObj is LayerGroup)
                    {
                        groupId = index;
                    }

                    toolWindow.Repaint();

                    Event.current.Use();
                }
            }


            if (draggedId >= 0 &&
                Event.current.type == EventType.MouseUp &&
                rects.row.Contains(Event.current.mousePosition))
            {
                if (index == groupId)
                {
                    if (layerObj is LayerGroup group)
                    {
                        Layer draggedLayer = config.GetLayer(draggedId);

                        config.AddLayerToGroup(draggedLayer, group);

                        EditorUtility.SetDirty(config);
                    }
                }

                draggedId = -1;
                groupId = -1;
                Event.current.Use();
                toolWindow.Repaint();
            }
        }

        private static void DrawButtons(int index, RowRects rects, LayerObject layerObj)
        {
            GUIContent eyeIcon = EditorGUIUtility.IconContent(
                                    layerObj.visible
                                        ? "animationvisibilitytoggleon"
                                        : "animationvisibilitytoggleoff");

            if (GUI.Button(rects.visibility, eyeIcon, EditorStyles.iconButton))
            {
                Undo.RecordObject(config, "Toggle Layer Visibility");

                layerObj.visible = !layerObj.visible;

                EditorUtility.SetDirty(config);
                toolWindow.Repaint();
            }

            if (layerObj is LayerGroup group && !group.IsEmpty)
            {
                bool expand = group.expanded;
                expand = EditorGUI.Foldout(
                    rects.expand,
                    group.expanded,
                    GUIContent.none);

                group.Expand(expand);
            }

            if (layerObj != null && index > 0 &&
                GUI.Button(rects.moveUp, EditorGUIUtility.IconContent("scrollup"), EditorStyles.iconButton))
            {
                config.MoveLayerUp(index);
                config.selectedLayer--;
                EditorUtility.SetDirty(config);
                toolWindow.Repaint();
            }

            if (layerObj != null && index < config.ExpandedLayers.Count - 1 &&
                GUI.Button(rects.moveDown, EditorGUIUtility.IconContent("scrolldown"), EditorStyles.iconButton))
            {
                config.MoveLayerDown(index);
                config.selectedLayer++;
                EditorUtility.SetDirty(config);
                toolWindow.Repaint();
            }
        }
    }
}
