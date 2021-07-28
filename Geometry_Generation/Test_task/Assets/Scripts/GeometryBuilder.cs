using System.Collections;
using UnityEngine;

public class GeometryBuilder : MonoBehaviour, IGeometryBuilder
{
    public int Faces_count = 3;

    public float
        Lenght = 1,
        Height = 1,
        Widht = 1,
        Radius = 1;

    public GameObject Figure;

    private GameObject _current_figure;

    private Material _current_material;

    private IGeometry geometry;

    public delegate void FigureError(string name);
    public static event FigureError OnFigureError;

    //
    private int[] data = new int[3] { 0, 0, 0 };

    public int GetVertices { get { return data[0]; } }
    public int GetTriangles { get { return data[1]; } }
    public int GetQuads { get { return data[2]; } }
    //

    public void CreateParallelepiped(float lenght, float height, float widht)
    {
        if(lenght > 0 && height > 0 && widht > 0)
        {
            Lenght = lenght;
            Height = height;
            Widht = widht;

            geometry = GetComponent<Parallelepiped>();

            StartCoroutine(CreateGeometry("Parallelepiped"));
        }
        else
        {
            OnFigureError?.Invoke("Parallelepiped");
        }
    }

    public void CreatePrism(float height, float radius, int faces_count)
    {
        if(height > 0 && radius > 0 && faces_count >= 3)
        {
            Height = height;
            Radius = radius;
            Faces_count = faces_count;

            geometry = GetComponent<Prism>();

            StartCoroutine(CreateGeometry("Prism"));
        }
        else
        {
            OnFigureError?.Invoke("Prism");
        }
    }

    public void CreateSphere(float radius, int faces_count)
    {
        if(radius > 0 && faces_count >= 3)
        {
            Radius = radius;
            Faces_count = faces_count;

            geometry = GetComponent<Sphere>();

            StartCoroutine(CreateGeometry("Sphere"));
        }
        else
        {
            OnFigureError?.Invoke("Sphere");
        }
    }

    public void CreateCapsule(float height, float radius, int faces_count)
    {
        if (radius > 0 && height > radius * 2 && faces_count >= 3)
        {
            Height = height;
            Radius = radius;
            Faces_count = faces_count;

            geometry = GetComponent<Capsule>();

            StartCoroutine(CreateGeometry("Capsule"));
        }
        else
        {
            OnFigureError?.Invoke("Capsule");
        }
    }

    private IEnumerator CreateGeometry(string figure_name)
    {
        _current_figure = Instantiate(Figure, new Vector3(0, 0, 0), Quaternion.identity);
        _current_figure.name = figure_name;

        geometry.Create();

        Mesh mesh = _current_figure.GetComponent<MeshFilter>().mesh;

        mesh.vertices = geometry.GetVertices;
        mesh.triangles = geometry.GetTriangles;

        mesh.RecalculateNormals();

        data = geometry.GetData;

        OnFigureError?.Invoke("Complete");

        _current_material = _current_figure.GetComponent<MeshRenderer>().material;

        yield return null;
    }

    public void ChangedColor(Color color)
    {
        _current_material.color = color;
    }

    public void DestroyFigure()
    {
        Destroy(_current_figure);

        //TEST
        geometry.Clean();
    }
}
