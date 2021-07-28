using Zenject;

public class Main_sceneInstaller : MonoInstaller
{
    public GeometryBuilder geometryBuilder;

    public override void InstallBindings()
    {
        BindGeometryBuilder();
        BindIGeometryBuilder();
    }

    private void BindGeometryBuilder()
    {
        Container
            .Bind<GeometryBuilder>()
            .FromInstance(geometryBuilder)
            .AsSingle();
    }

    private void BindIGeometryBuilder()
    {
        Container
            .Bind<IGeometryBuilder>()
            .FromInstance(geometryBuilder)
            .AsSingle();
    }
}
