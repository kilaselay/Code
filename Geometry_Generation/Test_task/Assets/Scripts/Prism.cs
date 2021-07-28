using UnityEngine;

public class Prism : Geometry, IGeometry
{
    protected int belts_count;//Кол-во поясов, содержащих боковые грани

    protected float
        radius,
        height;

    public override void Create()
    {
        radius = geometryBuilder.Radius;
        height = geometryBuilder.Height;

        faces_count = geometryBuilder.Faces_count;
        belts_count = 1;

        GenerateVertices();
        GenerateTriangles();

        //
        GismosColor = Color.green;
        //
    }

    protected override void GenerateVertices()
    {
        vertices = new Vector3[faces_count * 2 + 2];//+2 - центральные точки оснований

        float angle = 360 / faces_count;
        float current_angle = angle;

        y = 0;

        vertices[0] = new Vector3(0, 0, 0);//Центральная точка нижнего основания

        for (int i = 1; i < vertices.Length - 1; i++)
        {
            x = Mathf.Cos(current_angle * Mathf.Deg2Rad) * radius;
            z = Mathf.Sin(current_angle * Mathf.Deg2Rad) * radius;

            vertices[i] = new Vector3(x, y, z);

            if (i != faces_count)
            {
                current_angle += angle;
            }
            else
            {
                y = height;

                current_angle = angle;
            }
        }

        vertices[vertices.Length - 1] = new Vector3(0, height, 0);//Центральная точка верхнего основания

        Vertices_Count = vertices.Length;
    }

    protected override void GenerateTriangles()
    {
        int[] side_faces;

        int[] one_base;

        one_base = GenerateDownBase();

        side_faces = GenerateSideFaces();

        triangles = new int[side_faces.Length + one_base.Length * 2];

        one_base.CopyTo(triangles, 0);//Генерация нижнего основания
        side_faces.CopyTo(triangles, one_base.Length);//Генерация боковых граней

        one_base = GenerateUpBase();//Генерация верхнего основания
        one_base.CopyTo(triangles, side_faces.Length + one_base.Length);

        Triangle_Count = triangles.Length / 3;
        Quad_Count = faces_count * belts_count;
    }

    //Создание БОКОВЫХ ГРАНЕЙ
    protected virtual int[] GenerateSideFaces()
    {
        int[] side_faces = new int[faces_count * belts_count * 6];

        int[] quad; //Положение 2-х треугольников на одной грани (формирование по часовой)

        int finish = faces_count * belts_count + 1;//+1 - последняя вершина (центр верхнего основания)

        //i - счётчик номеров вершин, j - боковых поясов граней, vi - счётчик граней(2 треугольника - 6 вершин)
        //i = 1, т.к. 0 - вершина центра нижнего основания
        for (int i = 1, j = 1, vi = 0; i < finish; i++, vi += 6)
        {
            if (i != faces_count * j)
            {
                quad = GenerateQuad(i, i + faces_count, i + 1, i + faces_count + 1);
            }
            else //Формирование последней боковой грани в поясе
            {
                quad = GenerateQuad(i, i + faces_count, i - (faces_count - 1), i + 1);
                j++;
            }

            quad.CopyTo(side_faces, vi);
        }

        return side_faces;
    }

    //Создание треугольников НИЖНЕГО ОСНОВАНИЯ
    protected int[] GenerateDownBase()
    {
        int[] down_base = new int[faces_count * 3];

        //i = номер вершины треугольника, j-номер вершины основания
        for (int i = 0, j = 1; i < down_base.Length; i += 3, j++)
        {
            if (j != faces_count)
            {
                down_base[i] = j;
                down_base[i + 1] = j + 1;
                down_base[i + 2] = 0;//Точка в центре нижнего основания
            }
            else //Формирование последней грани основания
            {
                down_base[i] = j;
                down_base[i + 1] = 1;
                down_base[i + 2] = 0;
            }
        }

        return down_base;
    }

    //Создание треугольников ВЕРХНЕГО ОСНОВАНИЯ
    protected int[] GenerateUpBase()
    {
        int[] up_base = new int[faces_count * 3];

        int center = vertices.Length - 1;//Центральная точка верхнего основания

        int start_point = vertices.Length - faces_count - 1;//Точка начала формирования верхн. основания; -1 - центр основания
        int final_point = vertices.Length - 2;//Последняя точка, не считая центральной

        for (int i = 0, j = start_point; i < up_base.Length; i += 3, j++)
        {
            if (j != final_point)
            {
                up_base[i] = j;
                up_base[i + 1] = center;
                up_base[i + 2] = j + 1;
            }
            else //Формирование последней грани основания
            {
                up_base[i] = j;
                up_base[i + 1] = center;
                up_base[i + 2] = start_point;
            }
        }

        return up_base;
    }
}
