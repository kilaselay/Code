using UnityEngine;

public class Capsule : Sphere, IGeometry
{
    protected float additional_height;//Высота от центра капсулы до полусферы

    public override void Create()
    {
        radius = geometryBuilder.Radius;
        height = geometryBuilder.Height;
        faces_count = geometryBuilder.Faces_count;

        belts_count = faces_count - 2;

        GenerateVertices();
        GenerateTriangles();

        //
        GismosColor = Color.yellow;
        //
    }

    protected override void GenerateVertices()
    {
        if (belts_count % 2 == 0)
        {
            hemisphere_belts = belts_count / 2;
            belts_count += 1;
        }
        else
        {
            hemisphere_belts = (belts_count + 1) / 2;
            belts_count += 2;
        }

        distance = 90 / (hemisphere_belts + 1);

        angle = 360 / faces_count;

        additional_height = (height - radius * 2) / 2;

        Vector3[] hemisphere = new Vector3[faces_count * hemisphere_belts];
        Vector3[] central_part = new Vector3[faces_count * 2];

        vertices = new Vector3[hemisphere.Length * 2 + central_part.Length + 2];

        vertices[0] = new Vector3(0, -height / 2, 0);//Центральная вершина нижнего полюса

        hemisphere = GenerateDownHemisphere();
        hemisphere.CopyTo(vertices, 1);

        central_part = GenerateCenter();
        central_part.CopyTo(vertices, hemisphere.Length + 1);

        hemisphere = GenerateUpHemisphere();
        hemisphere.CopyTo(vertices, hemisphere.Length + central_part.Length + 1);

        vertices[vertices.Length - 1] = new Vector3(0, height / 2, 0);//Центральная вершина верхнего полюса

        Vertices_Count = vertices.Length;
    }

    //Создание точек центральной части капсулы
    protected Vector3[] GenerateCenter()
    {
        Vector3[] central_part = new Vector3[faces_count * 2];

        angle = 360 / faces_count;
        float current_angle = angle;

        y = -additional_height;

        for (int i = 0, j = 1; i < central_part.Length; i++)
        {
            x = Mathf.Cos(current_angle * Mathf.Deg2Rad) * radius;
            z = Mathf.Sin(current_angle * Mathf.Deg2Rad) * radius;

            central_part[i] = new Vector3(x, y, z);

            if (i != faces_count * j - 1)
            {
                current_angle += angle;
            }
            else
            {
                y = additional_height;

                current_angle = angle;

                j++;
            }
        }

        return central_part;
    }

    //Вычисление ВЫСОТЫ промежуточного кольца вершин
    protected override float CalculateBeltHeight(float longitude)
    {
        if(longitude < 0)
        {
            additional_height = Mathf.Abs(additional_height);
            additional_height = -additional_height;
        }
        else
        {
            additional_height = Mathf.Abs(additional_height);
        }

        float mini_height = Mathf.Cos((90 - longitude) * Mathf.Deg2Rad) * radius + additional_height;

        return mini_height;
    }
}
