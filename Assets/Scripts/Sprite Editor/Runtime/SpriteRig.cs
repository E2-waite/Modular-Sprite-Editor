using System.Collections.Generic;
using Haztech.SpriteEditor.Data;
using UnityEngine;

namespace Haztech.SpriteEditor.Runtime
{
    public class SpriteRig : MonoBehaviour
    {
        public Transform body;
        public SpriteConfig config;
        private List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        public void Build()
        {
            Clear();
            for (int i = 0; i < config.LayerCount; i++)
            {
                Layer layer = config.GetLayer(i);
                if (layer == null) continue;

                GameObject layerObj = new GameObject(layer.name);
                layerObj.transform.parent = transform;

                SpriteRenderer rend = layerObj.AddComponent<SpriteRenderer>();
                rend.sprite = config.GetSprite(i);
                rend.color = config.GetColor(i);
                rend.sortingOrder = config.LayerCount - i;
                renderers.Add(rend);
            }
        }

        public void Clear()
        {
            for (int i = renderers.Count - 1; i >= 0; i--)
            {
                if (renderers[i] != null)
                {
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                        DestroyImmediate(renderers[i].gameObject);
                    else
#endif
                        Destroy(renderers[i].gameObject);
                }
            }
            renderers.Clear();
        }
    }
}
