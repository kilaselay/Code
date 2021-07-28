using UnityEngine;

interface IGeometry
{
    public Vector3[] GetVertices { get; }

    public int[] GetTriangles { get; }

    //���������� ����� ������(0), �������������(1), 4-�������� ������(2)
    public int[] GetData { get; }
    //

    public abstract void Create();

    //TEST //������� ������ ��� ���������� Gizmos
    public void Clean();
    //
}
