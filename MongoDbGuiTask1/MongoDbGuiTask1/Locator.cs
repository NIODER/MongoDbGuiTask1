using MongoDbGuiTask1.ViewModel;

namespace MongoDbGuiTask1
{
    internal class Locator
    {
        public MainViewModel MainViewModel { get; set; }

        public Locator()
        {
            MainViewModel = new();
        }
    }
}
