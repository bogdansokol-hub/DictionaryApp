namespace DictionaryApp;

public partial class MainPage : ContentPage
{
    private WordsViewModel vm;

    public MainPage()
    {
        InitializeComponent();
        vm = new WordsViewModel();
        BindingContext = vm;
    }
}
