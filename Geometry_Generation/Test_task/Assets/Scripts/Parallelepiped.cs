using UnityEngine;

public class Parallelepiped : Geometry, IGeometry
{
    float lenght, height, widht;

    public override void Create()
    {
        lenght = geometryBuilder.Lenght;
        height = geometryBuilder.Height;
        widht = geometryBuilder.Widht;

        faces_count = 4;

        GenerateVertices();
        GenerateTriangles();

        //
        GismosColor = Color.red;
        //
    }

    protected override void GenerateVertices()
    {
        vertices = new Vector3[faces_count * 2];

        x = lenght / 2;
        y = 0;
        z = widht / 2;

        for (int i = 0; i < vertices.Length; i += 4)
        {
            vertices[i] = new Vector3(-x, y, -z);
            vertices[i + 1] = new Vector3(x, y, -z);
            vertices[i + 2] = new Vector3(x, y, z);
            vertices[i + 3] = new Vector3(-x, y, z);

            y = height;
        }

        Vertices_Count = vertices.Length;
    }

    protected override void GenerateTriangles()
    {
        int[] side_faces;
        int[] bases;

        side_faces = GenerateSideFaces();
        bases = GenerateBases();

        triangles = new int[side_faces.Length + bases.Length];

        side_faces.CopyTo(triangles, 0);
        bases.CopyTo(triangles, side_faces.Length);

        Triangle_Count = triangles.Length / 3;
        Quad_Count = Triangle_Count / 2;
    }

    //Создание боковых граней
    protected int[] GenerateSideFaces()
    {
        int[] side_faces = new int[faces_count * 6];

        int[] quad;

        //i - счётчик номеров вершин, vi - счётчик граней(2 треугольника - 6 вершин)
        for (int i = 0, vi = 0; i < faces_count; i++, vi += 6)
        {
            if (i != faces_count - 1)
            {
                quad = GenerateQuad(i, i + faces_count, i + 1, i + faces_count + 1);
            }
            else //Формирование последней боковой грани
            {
                quad = GenerateQuad(i, i + faces_count, i - (faces_count - 1), i + 1);
            }

            quad.CopyTo(side_faces, vi);
        }

        return side_faces;
    }

    protected int[] GenerateBases()
    {
        int[] bases = new int[12];

        int[] quad;

        //Нижнее основание (порядок вершин против часовой)
        quad = GenerateQuad(0, 1, 3, 2);
        quad.CopyTo(bases, 0);

        //Верхнее основание
        quad = GenerateQuad(4, 7, 5, 6);
        quad.CopyTo(bases, 6);

        return bases;
    }
}
