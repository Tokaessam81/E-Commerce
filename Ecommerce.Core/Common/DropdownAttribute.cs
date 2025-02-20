[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DropdownAttribute : Attribute
{
    public Type EntityType { get; }

    public DropdownAttribute(Type entityType)
    {
        EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
    }
}