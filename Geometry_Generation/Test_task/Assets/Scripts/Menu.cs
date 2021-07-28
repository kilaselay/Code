using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Menu : MonoBehaviour
{
    public GameObject ColorMenu;

    public Button[] Figures = new Button[4];
    public Button[] PredefinedColors = new Button[4];

    public Button Create;
    public Button Delete;

    public InputField Lenght;
    public InputField Height;
    public InputField Widht;
    public InputField Radius;
    public InputField FacesCount;

    public InputField[] RGB = new InputField[3];

    public Text Description;

    private IGeometryBuilder geometryBuilder;

    private int ID;

    private float
        _lenght,
        _height,
        _width,
        _radius;

    private int _faces_count;

    [Inject]
    private void Construct(IGeometryBuilder _geometryBuilder)
    {
        geometryBuilder = _geometryBuilder;
    }

    private void Start()
    {
        GeometryBuilder.OnFigureError += ShowWarning;
    }

    public void SelectFigure(int figure_ID)
    {
        ID = figure_ID;

        Description.color = Color.green;

        switch (ID)
        {
            case 0:
                Description.text = "Параллелепипед";
                Lenght.interactable = true;
                Height.interactable = true;
                Widht.interactable = true;
                Radius.interactable = false;
                FacesCount.interactable = false;
                break;
            case 1:
                Description.text = "Призма";
                Lenght.interactable = false;
                Height.interactable = true;
                Widht.interactable = false;
                Radius.interactable = true;
                FacesCount.interactable = true;
                break;
            case 2:
                Description.text = "Сфера";
                Lenght.interactable = false;
                Height.interactable = false;
                Widht.interactable = false;
                Radius.interactable = true;
                FacesCount.interactable = true;
                break;
            case 3:
                Description.text = "Капсула";
                Lenght.interactable = false;
                Height.interactable = true;
                Widht.interactable = false;
                Radius.interactable = true;
                FacesCount.interactable = true;
                break;
        }

        Create.gameObject.SetActive(true);
    }

    public void CreateFigure()
    {
        switch (ID)
        {
            case 0:
                if (CheckParallelepiped())
                {
                    LockedMenu();
                    geometryBuilder.CreateParallelepiped(_lenght, _height, _width);
                }
                break;
            case 1:
                if (CheckPrismCapsule())
                {
                    LockedMenu();
                    geometryBuilder.CreatePrism(_height, _radius, _faces_count);
                }
                break;
            case 2:
                if (CheckSphere())
                {
                    LockedMenu();
                    geometryBuilder.CreateSphere(_radius, _faces_count);
                }
                break;
            case 3:
                if (CheckPrismCapsule())
                {
                    LockedMenu();
                    geometryBuilder.CreateCapsule(_height, _radius, _faces_count);
                }
                break;

        }
    }

    public void DeleteFigure()
    {
        geometryBuilder.DestroyFigure();

        UnlockedMenu();

        ColorMenu.SetActive(false);

        Description.text = "";
    }

    public void SetColor()
    {
        if (IsInput(RGB))
        {
            int r = int.Parse(RGB[0].text);
            int g = int.Parse(RGB[1].text);
            int b = int.Parse(RGB[2].text);

            Color color = new Color(r, g, b);

            geometryBuilder.ChangedColor(color);
        }
    }

    public void SetPredefinedColors(string color_name)
    {
        Color color = new Color();

        switch (color_name)
        {
            case "red":
                color = Color.red;
                break;
            case "green":
                color = Color.green;
                break;
            case "blue":
                color = Color.blue;
                break;
            case "yellow":
                color = Color.yellow;
                break;
        }

        RGB[0].text = color.r.ToString();
        RGB[1].text = color.g.ToString();
        RGB[2].text = color.b.ToString();

        geometryBuilder.ChangedColor(color);
    }

    private bool CheckParallelepiped()
    {
        InputField[] inputs = new InputField[3];

        inputs[0] = Lenght;
        inputs[1] = Height;
        inputs[2] = Widht;

        if (IsInput(inputs))
        {
            _lenght = float.Parse(Lenght.text);
            _height = float.Parse(Height.text);
            _width = float.Parse(Widht.text);

            return true;
        }
        else
            return false;
    }

    private bool CheckPrismCapsule()
    {
        InputField[] inputs = new InputField[3];

        inputs[0] = Height;
        inputs[1] = Radius;
        inputs[2] = FacesCount;

        if (IsInput(inputs))
        {
            _height = float.Parse(Height.text);
            _radius = float.Parse(Radius.text);

            _faces_count = int.Parse(FacesCount.text);

            return true;
        }
        else
            return false;
    }

    private bool CheckSphere()
    {
        InputField[] inputs = new InputField[2];

        inputs[0] = Radius;
        inputs[1] = FacesCount;

        if (IsInput(inputs))
        {
            _radius = float.Parse(Radius.text);

            _faces_count = int.Parse(FacesCount.text);

            return true;
        }
        else
            return false;
    }

    //Проверка ввода
    private bool IsInput(InputField[] input)
    {
        for(int i=0; i<input.Length; i++)
        {
            if(input[i].text == "")
            {
                Description.color = Color.yellow;
                Description.text = "Не все поля были заполнены";
                return false;
            }
        }

        return true;
    } 

    private void LockedMenu()
    {
        Lenght.interactable = false;
        Height.interactable = false;
        Widht.interactable = false;
        Radius.interactable = false;
        FacesCount.interactable = false;

        for (int i = 0; i < Figures.Length; i++)
        {
            Figures[i].interactable = false;
        }

        Create.gameObject.SetActive(false);
        Delete.gameObject.SetActive(true);
    }

    private void UnlockedMenu()
    {
        for (int i = 0; i < Figures.Length; i++)
        {
            Figures[i].interactable = true;
        }

        Delete.gameObject.SetActive(false);
    }

    private void ShowData()
    {
        Description.color = Color.blue;
        Description.text = 
            "Вершины: " + geometryBuilder.GetVertices + 
            "; Треугольники: " + geometryBuilder.GetTriangles +
            "; 4-угольные грани: " + geometryBuilder.GetQuads;
    }

    private void ShowWarning(string name)
    {
        Description.color = Color.yellow;

        switch (name)
        {
            case "Complete":
                ShowData();
                ColorMenu.SetActive(true);
                break;
            case "Parallelepiped":
                Description.text = "Все параметры параллелепипеда должны быть положительными";
                UnlockedMenu();
                break;
            case "Prism":
                Description.text = "Все параметры призмы должны быть положительными \n" +
                    "Количество граней должно быть не меньше трёх";
                UnlockedMenu();
                break;
            case "Sphere":
                Description.text = "Все параметры сферы должны быть положительными \n" +
                    "Оптимальное количество граней от 8";
                UnlockedMenu();
                break;
            case "Capsule":
                Description.text = "Все параметры капсулы должны быть положительными \n" +
                    "Высота должна быть минимум в 2+ раза больше радиуса. Оптимальное количество граней от 8";
                UnlockedMenu();
                break;
        }
    }

    private void OnDestroy()
    {
        GeometryBuilder.OnFigureError -= ShowWarning;
    }
}
