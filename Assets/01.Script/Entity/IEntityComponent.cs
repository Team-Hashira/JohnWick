public interface IEntityComponent
{
    public void Initialize(Entity entity);
}

public interface IAfterInitComponent
{
    public void AfterInit();
}

public interface IEntityDisposeComponent
{
    public void Dispose();
}
