Unity 2020.3.11f1 (LTS)
MS Visual Studio 2019 Community
Zenject (Extenject) 9.2.0

//Управление камерой в режиме игры не реализовано
//Просматривать результат построения удобнее в режиме сцены с модификатором "Shaded Wirefarme"
//Каждая точка модели выделяется средствами Gizmos (можно закомментировать в базовом классе Geometry)

//Все точки строятся в порядке слева-направо снизу-вверх
//Во всех фигурах, кроме прямоугольника, 0-точка - вершина нижнего основания, последняя точка - вершина верхнего основания

//-----

//Реализуется всеми классами-фигурами.
interface IGeometry
{
    public Vector3[] GetVertices { get; } //Возвращает массив вершин

    public int[] GetTriangles { get; } //Возврашает массив треугольников

    
    public int[] GetData { get; } //Возвращает число вершин(0), треугольников(1), 4-угольных граней(2) (ТЕСТОВЫЙ)
    

    public abstract void Create(); //Создаёт фигуру. Индивидуален для каждого класса-фигуры

    
    public void Clean(); //Очищает данные для обновления Gizmos (ТЕСТОВЫЙ)
    
}

//-----

//Базовый класс для всех конкретных классов-фигур
public abstract class Geometry : MonoBehaviour, IGeometry
{
	public Vector3[] GetVertices { get { return vertices; } }

    public int[] GetTriangles { get { return triangles; } }

    public int[] GetData {}
	
	public void Clean() {}
	
	[Inject]
    protected void Construct(GeometryBuilder _geometryBuilder)
	{
		//Имитирует конструктор класса-фигуры
		//Принимает и кэширует конкретный экземляр класса GeometryBuilder со сцены
	}
	
	public abstract void Create();

    protected abstract void GenerateVertices(); //Служит для построения вершин. Индивидуален для каждого класса-фигуры

    protected abstract void GenerateTriangles(); //Служит для построения треугольников. Идентичен в некоторых фигурах
	
	protected int[] GenerateQuad(int v0, int v1, int v2, int v3)
	{
		//Создаёт 4-х угольную грань из двух треугольников
		//Принимает номера 4-х вершин для построения
		//Возврашает массив с номерами вершин 2-х треугольников (6 значений)
	}
	
	private void OnDrawGizmos()
	{
		//Отрисовывает сферы радиусом 0.05 на вершинах
	}
}

//-----

//Служит для создания параллелепипеда
public class Parallelepiped : Geometry, IGeometry
{
	public override void Create(){}
	
	protected override void GenerateVertices(){}
	
	protected override void GenerateTriangles(){}
	
	protected int[] GenerateSideFaces()
	{
		//Служит для создания боковых граней
		//Возвращает массив с номерами вершин треугольников
	}
	
	protected int[] GenerateBases()
	{
		//Служит для создания оснований
		//Возвращает массив с номерами вершин треугольников оснований
	}
}

//-----

//Служит для создания призмы
public class Prism : Geometry, IGeometry
{
	public override void Create(){}
	
	protected override void GenerateVertices(){}
	
	protected override void GenerateTriangles()
	{
		//Используется без изменений в производных классах сферы и капсулы
	}
	
	protected virtual int[] GenerateSideFaces()
	{
		//Служит для создания боковых граней
		//Возвращает массив с номерами вершин треугольников
		//Идентичен в производных классах сферы и капсулы
	}
	
	protected int[] GenerateDownBase()
	{
		//Служит для создания нижнего основания из треугольников с общей вершиной в центре основания
		//Построение треугольника против часовой
		//Возвращает массив номеров вершин треугольников нижнего основания
		//Используется также в производных классах сферы и капсулы
	}
	
	protected int[] GenerateUpBase()
	{
		//Служит для создания верхнего основания из треугольников
		//Построение треугольника по часовой
		//Возвращает массив номеров вершин треугольников верхнего основания
		//Используется также в производных классах сферы и капсулы
	}
}

//-----

//Служит для построения сферы
public class Sphere : Prism, IGeometry
{
	public override void Create(){}
	
	protected override void GenerateVertices(){}
	
	protected Vector3[] GenerateDownHemisphere()
	{
		//Служит для построения вершин граней нижней полусферы без основания и кольца(колец) нулевой параллели
		//Возвращает массив координат вершин
		//Также используется капсулой
	}
	
	protected Vector3[] GenerateCentralBelt()
	{
		//Служит для построения вершин нулевой параллели или 2-х центральных параллелей (в зависимости от чётности количества секторов сглаживания)
		//Возвращает массив координат вершин
	}
	
	protected Vector3[] GenerateUpHemisphere()
	{
		//Служит для построения вершин граней верхней полусферы без основания и кольца(колец) нулевой параллели
		//Возвращает массив координат вершин
		//Также используется капсулой
	}
	
	protected float CalculateBeltRadius(float latitude)
	{
		//Служит для рассчёта радиуса кольца вершин на параллели сферы
		//Принимает широту кольца
		//Возвращает радиус кольца
		//Используется без изменений в классе-капсуле
	}
	
	protected virtual float CalculateBeltHeight(float longitude)
	{
		//Служит для рассчёта высоты кольца вершин на меридиане сферы
		//Принимает долготу кольца
		//Возвращает высоту кольца
		//Переопределяется в классе-капсуле
	}
}

//-----

//Служит для построения капсулы
public class Capsule : Sphere, IGeometry
{
	public override void Create(){}
	
	protected override void GenerateVertices(){}
	
	protected Vector3[] GenerateCenter()
	{
		//Служит для создания точек колец нулевых меридианов каждой полусферы
		//Радиус меридиана равен заданному. Кольца являются границами центральной части капсулы
		//Возвращает массив координат точек центральных колец
	}
	
	protected override float CalculateBeltHeight(float longitude)
	{
		//Дополняет реализацию рассчёта высоты
	}
}

//-----

//Реализуется классом GeometryBuilder
//Обеспечивает интерфейс взаимодействия для классов не-фигур
//Изолирует от доступа к другим публичным параметрам класса
interface IGeometryBuilder
{
	public int GetVertices { get; } //Возвращает количество вершин построенной модели (ТЕСТОВЫЙ)
	
    public int GetTriangles { get; } //Возвращает количество треугольников построенной модели (ТЕСТОВЫЙ)
	
    public int GetQuads { get; } //Возвращает количество 4-угольных граней построенной модели (ТЕСТОВЫЙ)
	
	public void CreateParallelepiped(float lenght, float height, float widht); //Служит для инициализации построения прямоугольника

    public void CreatePrism(float height, float radius, int faces_count); //Служит для инициализации построения призмы

    public void CreateSphere(float radius, int faces_count); //Служит для инициализации построения сферы

    public void CreateCapsule(float height, float radius, int faces_count); //Служит для инициализации построения капсулы

    public void ChangedColor(Color color); //Служит для смены цвета материала фигуры

    public void DestroyFigure(); //Служит для удаления фигуры со сцены
}

//-----

//Служит связующим классом между классами Geometry и классом Menu
public class GeometryBuilder : MonoBehaviour, IGeometryBuilder
{
	public int GetVertices { get; }
	
    public int GetTriangles { get; }
	
    public int GetQuads { get; }
	
	public void CreateParallelepiped(float lenght, float height, float widht);

    public void CreatePrism(float height, float radius, int faces_count);

    public void CreateSphere(float radius, int faces_count);

    public void CreateCapsule(float height, float radius, int faces_count);

    public void ChangedColor(Color color);

    public void DestroyFigure();
	
	private IEnumerator CreateGeometry(string figure_name)
	{
		//Служит для реализации общего для всех классов-фигур алгоритма построения
		//Создаёт фигуру на сцене
		//Принимает имя, выдаваемое новой фигуре
	}
}

//-----

//Служит для передачи значений с графического интерфейса классу GeometryBuilder по интерфейсу
public class Menu : MonoBehaviour
{
	[Inject]
    private void Construct(IGeometryBuilder _geometryBuilder)
	{
		//Имитирует конструктор класса
		//Принимает и кэширует конкретный экземпляр GeometryBuilder со сцены
	}
	
	private void Start(){}
	
	public void SelectFigure(int figure_ID)
	{
		//Служит для управления элементами GUI
		//Предоставляет для ввода обязательные поля для выбранной фигуры
		//Принимает номер фигуры
	}
	
	public void CreateFigure()
	{
		//Отправляет параметры для создания конкретной фигуры
	}
	
	public void DeleteFigure()
	{
		//Отправляет запрос на удаление фигуры
	}
	
	public void SetColor()
	{
		//Отправляет запрос на смену цвета фигуры
	}
	
	public void SetPredefinedColors(string color_name)
	{
		//Отправляет запрос на смену цвета из числа предопределённых цветов
		//Принимает имя предопределённого цвета
	}
	
	private bool CheckParallelepiped()
	{
		//Проверяет параметры для параллелепипеда, считанные с полей ввода, на наличие/отсутствие введённого значения
		//Возвращает true если все поля заполнены, иначе false
	}
	
	private bool CheckPrismCapsule()
	{
		//Проверяет параметры для сферы или капсулы, считанные с полей ввода, на наличие/отсутствие введённого значения
		//Возвращает true если все поля заполнены, иначе false
	}
	
	private bool CheckSphere()
	{
		//Проверяет параметры для сферы, считанные с полей ввода, на наличие/отсутствие введённого значения
		//Возвращает true если все поля заполнены, иначе false
	}
	
	private bool IsInput(InputField[] input)
	{
		//Проверяет полученные поля ввода на наличие/отсутствие введённого значения
		//Принимает массив полей ввода
		//Возвращает true если все поля заполнены, иначе false
	}
	
	private void LockedMenu()
	{
		//Ограничивает доступ к меню создания фигур, после появления на сцене фигуры
	}
	
	private void UnlockedMenu()
	{
		//Открывает доступ к меню создания фигур, после удаления фигуры со сцены
	}
	
	private void ShowData()
	{
		//Выводит данные по количеству вершин, треугольников и 4-угольных граней построенной фигуры
	}
	
	private void ShowWarning(string name)
	{
		//Выводит предупреждение о некорректно введённых данных для фигуры
		//Принимает имя фигуры
	}
	
	private void OnDestroy(){}
}

//-----

[Zenject]

//Служит для внедрения зависимостей главной сцены
public class Main_sceneInstaller : MonoInstaller
{
	public override void InstallBindings(){}
	
	private void BindGeometryBuilder()
	{
		//Связывает класс GeometryBuilder со сцены с классами-фигурами
		//Срабатывает по запросу класса
	}
	
	private void BindIGeometryBuilder()
	{
		//Связывает класс GeometryBuilder со сцены с классом-меню
		//Срабатывает по запросу интерфейса
	}
}

//

(Developed by N.Romanovskiy)