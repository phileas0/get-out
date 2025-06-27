using UnityEngine;

[ExecuteInEditMode]
public class MapBuilder : MonoBehaviour
{
    [Header("Größe einer Kachel (Unity Units)")]
    public float tileSize = 3f;

    [Header("Deine Tiles – in der Reihenfolge 1, 2, 3 …")]
    public GameObject[] tilePrefabs;

    [Header("Map-Layout (0=leer, 1=erstes Prefab, 2=zwei …)")]
    public int[,] layout = new int[,]
    {
        {1,1,1,1},
        {1,0,0,1},
        {1,2,3,1},
        {1,1,1,1}
    };

    public int rows => layout.GetLength(0);
    public int cols => layout.GetLength(1);

    [ContextMenu("Generate Map")]
    public void GenerateMap()
    {
        while(transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        for(int r = 0; r < rows; r++)
        {
            for(int c = 0; c < cols; c++)
            {
                int id = layout[r,c];
                if(id <= 0 || id > tilePrefabs.Length) continue;

                var go = Instantiate(
                  tilePrefabs[id-1],
                  transform
                );
                go.transform.localPosition = new Vector3(
                  c * tileSize,
                  0,
                  -r * tileSize
                );
            }
        }
    }
}
