namespace EvieEngine
{
    using UnityEngine;
    using UnityEditor;

    [InitializeOnLoad]
    public class DebugBoundingBoxDrawer
    {
        // Цвет #9B87D7
        private static readonly Color debugColor = new Color(0x9B / 255f, 0x87 / 255f, 0xD7 / 255f, 1f);

        static DebugBoundingBoxDrawer()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            EditorApplication.hierarchyChanged += SceneView.RepaintAll;
            EditorApplication.update += SceneView.RepaintAll;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            GameObject[] debugObjects = GameObject.FindGameObjectsWithTag("Evie_Debug");

            Handles.color = debugColor;

            foreach (GameObject obj in debugObjects)
            {
                if (obj == null) continue;

                Bounds bounds = GetBounds(obj);

                // Если нет рендереров вообще — рисуем куб 1x1x1 вокруг позиции
                if (bounds.size == Vector3.zero)
                {
                    bounds = new Bounds(obj.transform.position, Vector3.one);
                }

                DrawWireCube(bounds.center, bounds.size);
            }
        }

        // Получает объединённые границы объекта и всех его дочерних рендереров
        private static Bounds GetBounds(GameObject obj)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(true); // true = включая неактивные

            if (renderers.Length == 0)
            {
                return new Bounds(); // size = (0,0,0) — обозначает "нет границ"
            }

            Bounds bounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        // Рисует проволочный куб по центру и размеру
        private static void DrawWireCube(Vector3 center, Vector3 size)
        {
            Vector3 halfSize = size * 0.5f;

            // 8 углов куба
            Vector3[] corners = new Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                corners[i] = center + new Vector3(
                    (i & 1) == 0 ? -halfSize.x : halfSize.x,
                    (i & 2) == 0 ? -halfSize.y : halfSize.y,
                    (i & 4) == 0 ? -halfSize.z : halfSize.z
                );
            }

            // 12 рёбер куба
            int[,] edges = new int[,]
            {
                { 0, 1 }, { 0, 2 }, { 0, 4 },
                { 1, 3 }, { 1, 5 },
                { 2, 3 }, { 2, 6 },
                { 3, 7 },
                { 4, 5 }, { 4, 6 },
                { 5, 7 },
                { 6, 7 }
            };

            for (int i = 0; i < edges.GetLength(0); i++)
            {
                Handles.DrawLine(corners[edges[i, 0]], corners[edges[i, 1]]);
            }
        }
    }
}