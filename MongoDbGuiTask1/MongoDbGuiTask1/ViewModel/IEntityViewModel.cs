using Database.Entities;

namespace MongoDbGuiTask1.ViewModel
{
    internal interface IEntityViewModel
    {
        DbEntity GetEntity();
    }
}
