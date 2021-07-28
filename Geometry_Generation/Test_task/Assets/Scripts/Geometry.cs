using UnityEngine;
using Zenject;

public abstract class Geometry : MonoBehaviour, IGeometry
{
    protected GeometryBuilder geometryBuilder;

    protected int faces_count;// Кол-во боковых граней

    protected float x, y, z;

    protected Vector3[] vertices;

    protected int[] triangles;

    protected int Vertices_Count, Triangle_Count, Quad_Count;

    //
    protected Color GismosColor = Color.red;
    //

    public Vector3[] GetVertices { get { return vertices; } }

    public int[] GetTriangles { get { return triangles; } }

    public int[] GetData { 
        get 
        {
            int[] data = new int[3] { Vertices_Count, Triangle_Count, Quad_Count };
            return data;
        } 
    }

    //TEST
    public void Clean()
    {
        vertices = null;
        triangles = null;
    }

    [Inject]
    protected void Construct(GeometryBuilder _geometryBuilder)
    {
        geometryBuilder = _geometryBuilder;
    }

    public abstract void Create();

    protected abstract void GenerateVertices();

    protected abstract void GenerateTriangles();

    //Создание 4-угольной грани из 2-х треугольников
    //Принимает 4 вершины для формирования грани, возвращает грань
    protected int[] GenerateQuad(int v0, int v1, int v2, int v3)
    {
        int[] quad = new int[6];

        quad[0] = v0;
        quad[1] = quad[5] = v1;
        quad[2] = quad[4] = v2;
        quad[3] = v3;

        return quad;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        Gizmos.color = GismosColor;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.05f);
        }
    }
}


