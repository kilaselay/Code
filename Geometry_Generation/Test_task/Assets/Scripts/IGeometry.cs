using UnityEngine;

interface IGeometry
{
    public Vector3[] GetVertices { get; }

    public int[] GetTriangles { get; }

    //Возвращает число вершин(0), треугольников(1), 4-угольных граней(2)
    public int[] GetData { get; }
    //

    public abstract void Create();

    //TEST //Очищает данные для обновления Gizmos
    public void Clean();
    //
}
