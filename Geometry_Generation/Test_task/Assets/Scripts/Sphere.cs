using UnityEngine;

public class Sphere : Prism, IGeometry
{
    protected int hemisphere_belts;//Количество поясов граней в полусфере
    protected int center_belts;//Количество колец вершин в центральном поясе

    protected float angle;//Величина угла многогранника
    protected float distance;//Величина промежуточного радиуса и высоты кольца вершин

    public override void Create()
    {
        radius = geometryBuilder.Radius;
        faces_count = geometryBuilder.Faces_count;

        belts_count = faces_count - 2;

        GenerateVertices();
        GenerateTriangles();

        //
        GismosColor = Color.blue;
        //
    }

    protected override void GenerateVertices()
    {
        if (belts_count % 2 == 0)
        {
            center_belts = 1;
            hemisphere_belts = belts_count / 2;
        }
        else
        {
            center_belts = 2;
            hemisphere_belts = (belts_count - 1) / 2;
        }

        distance = 90 / (hemisphere_belts + 1);//+1 кольцо вершин

        angle = 360 / faces_count;

        Vector3[] hemisphere = new Vector3[faces_count * hemisphere_belts];
        Vector3[] central_ring;

        hemisphere = GenerateDownHemisphere();
        central_ring = GenerateCentralBelt();

        vertices = new Vector3[hemisphere.Length * 2 + central_ring.Length + 2];

        vertices[0] = new Vector3(0, -radius, 0);//Центральная вершина нижнего полюса

        hemisphere.CopyTo(vertices, 1);
        central_ring.CopyTo(vertices, hemisphere.Length + 1);

        hemisphere = GenerateUpHemisphere();
        hemisphere.CopyTo(vertices, hemisphere.Length + central_ring.Length + 1);

        vertices[vertices.Length - 1] = new Vector3(0, radius, 0);//Центральная вершина верхнего полюса

        Vertices_Count = vertices.Length;
    }

    //Создание вершин НИЖНЕЙ ПОЛУСФЕРЫ
    protected Vector3[] GenerateDownHemisphere()
    {
        Vector3[] down_hemisphere = new Vector3[faces_count * hemisphere_belts];

        float current_angle = angle;

        y = CalculateBeltHeight(-distance * hemisphere_belts);

        float current_radius = CalculateBeltRadius(distance * hemisphere_belts);

        for (int i = 0, j = 1; i < down_hemisphere.Length; i++)
        {
            x = Mathf.Cos(current_angle * Mathf.Deg2Rad) * current_radius;
            z = Mathf.Sin(current_angle * Mathf.Deg2Rad) * current_radius;

            down_hemisphere[i] = new Vector3(x, y, z);

            if (i != faces_count * j - 1)
            {
                current_angle += angle;
            }
            else
            {
                current_angle = angle;

                y = CalculateBeltHeight(-distance * (hemisphere_belts - j));
                current_radius = CalculateBeltRadius(distance * (hemisphere_belts - j));

                j++;
            }
        }

        return down_hemisphere;
    }

    //Создание ЦЕНТРАЛЬНОГО КОЛЬЦА(колец) вершин
    protected Vector3[] GenerateCentralBelt()
    {
        Vector3[] central_ring = new Vector3[faces_count * center_belts];

        float current_angle = angle;

        float current_radius = CalculateBeltRadius(center_belts - 1);

        if (center_belts == 1)
        {
            y = 0;
        }
        else
        {
            float local_belt_height = 90 / (faces_count + 2);
            y = CalculateBeltHeight(-local_belt_height * (center_belts - 1));
        }

        for (int i = 0, j = 1; i < central_ring.Length; i++)
        {
            x = Mathf.Cos(current_angle * Mathf.Deg2Rad) * current_radius;
            z = Mathf.Sin(current_angle * Mathf.Deg2Rad) * current_radius;

            central_ring[i] = new Vector3(x, y, z);

            if (i != faces_count * j - 1)
            {
                current_angle += angle;
            }
            else
            {
                j++;

                y = Mathf.Abs(y);

                current_angle = angle;
            }
        }

        return central_ring;
    }

    //Создание вершин ВЕРХНЕЙ ПОЛУСФЕРЫ
    protected Vector3[] GenerateUpHemisphere()
    {
        Vector3[] up_hemisphere = new Vector3[faces_count * hemisphere_belts];

        float current_angle = angle;

        y = CalculateBeltHeight(distance);

        float current_radius = CalculateBeltRadius(distance);

        for (int i = 0, j = 1; i < up_hemisphere.Length; i++)
        {
            x = Mathf.Cos(current_angle * Mathf.Deg2Rad) * current_radius;
            z = Mathf.Sin(current_angle * Mathf.Deg2Rad) * current_radius;

            up_hemisphere[i] = new Vector3(x, y, z);

            if (i != faces_count * j - 1)
            {
                current_angle += angle;
            }
            else
            {
                j++;

                y = CalculateBeltHeight(distance * j);
                current_radius = CalculateBeltRadius(distance * j);

                current_angle = angle;
            }
        }

        return up_hemisphere;
    }

    //Вычисление РАДИУСА промежуточного кольца вершин
    protected float CalculateBeltRadius(float latitude)
    {
        float mini_radius = Mathf.Sin((90 - latitude) * Mathf.Deg2Rad) * radius;

        return mini_radius;
    }

    //Вычисление ВЫСОТЫ промежуточного кольца вершин
    protected virtual float CalculateBeltHeight(float longitude)
    {
        float mini_height = Mathf.Cos((90 - longitude) * Mathf.Deg2Rad) * radius;

        return mini_height;
    }
}
