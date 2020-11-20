namespace Common.Repository.Application.Events.Interfaces
{
    internal interface ICelebrityEvent
    {
        public string Name { get; }
        EntityBase Celebrate(EntityBase entityBase);
    }
}
