using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DictionaryApp;

public class WordsViewModel : INotifyPropertyChanged
{
    private const string FILE_NAME = "words.txt";
    private string _searchQuery = string.Empty;

    public ObservableCollection<string> Words { get; set; } = new();

    public string NewWord { get; set; }

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (_searchQuery != value)
            {
                _searchQuery = value;
                OnPropertyChanged();
                ApplySearch();
            }
        }
    }

    private List<string> AllWords = new();

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public WordsViewModel()
    {
        LoadWords();
    }

    public async void LoadWords()
    {
        string path = Path.Combine(FileSystem.AppDataDirectory, FILE_NAME);
        if (File.Exists(path))
        {
            AllWords = File.ReadAllLines(path).ToList();
            SortWords();
            RefreshVisible();
        }
    }

    public void AddWord()
    {
        if (string.IsNullOrWhiteSpace(NewWord)) return;
        AllWords.Add(NewWord.Trim());
        SortWords();
        SaveWords();
        ApplySearch();
        NewWord = string.Empty;
        OnPropertyChanged(nameof(NewWord));
    }

    private void SortWords()
    {
        AllWords = AllWords
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private void SaveWords()
    {
        string path = Path.Combine(FileSystem.AppDataDirectory, FILE_NAME);
        File.WriteAllLines(path, AllWords);
    }

    private void ApplySearch()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            RefreshVisible();
        }
        else
        {
            var filtered = AllWords
                .Where(x => x.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Words.Clear();
            foreach (var w in filtered) Words.Add(w);
        }
    }

    private void RefreshVisible()
    {
        Words.Clear();
        foreach (var w in AllWords) Words.Add(w);
    }

    public Command AddWordCommand => new Command(AddWord);
}
