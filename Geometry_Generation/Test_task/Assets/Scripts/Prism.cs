using UnityEngine;

public class Prism : Geometry, IGeometry
{
    protected int belts_count;//���-�� ������, ���������� ������� �����

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
        vertices = new Vector3[faces_count * 2 + 2];//+2 - ����������� ����� ���������

        float angle = 360 / faces_count;
        float current_angle = angle;

        y = 0;

        vertices[0] = new Vector3(0, 0, 0);//����������� ����� ������� ���������

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

        vertices[vertices.Length - 1] = new Vector3(0, height, 0);//����������� ����� �������� ���������

        Vertices_Count = vertices.Length;
    }

    protected override void GenerateTriangles()
    {
        int[] side_faces;

        int[] one_base;

        one_base = GenerateDownBase();

        side_faces = GenerateSideFaces();

        triangles = new int[side_faces.Length + one_base.Length * 2];

        one_base.CopyTo(triangles, 0);//��������� ������� ���������
        side_faces.CopyTo(triangles, one_base.Length);//��������� ������� ������

        one_base = GenerateUpBase();//��������� �������� ���������
        one_base.CopyTo(triangles, side_faces.Length + one_base.Length);

        Triangle_Count = triangles.Length / 3;
        Quad_Count = faces_count * belts_count;
    }

    //�������� ������� ������
    protected virtual int[] GenerateSideFaces()
    {
        int[] side_faces = new int[faces_count * belts_count * 6];

        int[] quad; //��������� 2-� ������������� �� ����� ����� (������������ �� �������)

        int finish = faces_count * belts_count + 1;//+1 - ��������� ������� (����� �������� ���������)

        //i - ������� ������� ������, j - ������� ������ ������, vi - ������� ������(2 ������������ - 6 ������)
        //i = 1, �.�. 0 - ������� ������ ������� ���������
        for (int i = 1, j = 1, vi = 0; i < finish; i++, vi += 6)
        {
            if (i != faces_count * j)
            {
                quad = GenerateQuad(i, i + faces_count, i + 1, i + faces_count + 1);
            }
            else //������������ ��������� ������� ����� � �����
            {
                quad = GenerateQuad(i, i + faces_count, i - (faces_count - 1), i + 1);
                j++;
            }

            quad.CopyTo(side_faces, vi);
        }

        return side_faces;
    }

    //�������� ������������� ������� ���������
    protected int[] GenerateDownBase()
    {
        int[] down_base = new int[faces_count * 3];

        //i = ����� ������� ������������, j-����� ������� ���������
        for (int i = 0, j = 1; i < down_base.Length; i += 3, j++)
        {
            if (j != faces_count)
            {
                down_base[i] = j;
                down_base[i + 1] = j + 1;
                down_base[i + 2] = 0;//����� � ������ ������� ���������
            }
            else //������������ ��������� ����� ���������
            {
                down_base[i] = j;
                down_base[i + 1] = 1;
                down_base[i + 2] = 0;
            }
        }

        return down_base;
    }

    //�������� ������������� �������� ���������
    protected int[] GenerateUpBase()
    {
        int[] up_base = new int[faces_count * 3];

        int center = vertices.Length - 1;//����������� ����� �������� ���������

        int start_point = vertices.Length - faces_count - 1;//����� ������ ������������ �����. ���������; -1 - ����� ���������
        int final_point = vertices.Length - 2;//��������� �����, �� ������ �����������

        for (int i = 0, j = start_point; i < up_base.Length; i += 3, j++)
        {
            if (j != final_point)
            {
                up_base[i] = j;
                up_base[i + 1] = center;
                up_base[i + 2] = j + 1;
            }
            else //������������ ��������� ����� ���������
            {
                up_base[i] = j;
                up_base[i + 1] = center;
                up_base[i + 2] = start_point;
            }
        }

        return up_base;
    }
}
