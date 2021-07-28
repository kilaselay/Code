using UnityEngine;

interface IGeometryBuilder
{
    //
    public int GetVertices { get; }
    public int GetTriangles { get; }
    public int GetQuads { get; }
    //

    public void CreateParallelepiped(float lenght, float height, float widht);

    public void CreatePrism(float height, float radius, int faces_count);

    public void CreateSphere(float radius, int faces_count);

    public void CreateCapsule(float height, float radius, int faces_count);

    public void ChangedColor(Color color);

    public void DestroyFigure();
}
