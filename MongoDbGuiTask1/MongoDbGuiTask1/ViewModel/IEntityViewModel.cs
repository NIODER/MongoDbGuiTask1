using Database.Entities;

namespace MongoDbGuiTask1.ViewModel
{
    internal delegate void DocumentUpdatedEventHandler();

    internal interface IEntityViewModel
    {
        event DocumentUpdatedEventHandler? DocumentUpdated;
        DbEntity GetEntity();
    }
}
