using System.Diagnostics;

using UniversalSorter.SortsLibrary.SortAlgorithms;

namespace UniversalSorter.SortsLibrary.Tests;

public class SortTests
{
    private static Random rnd = new Random();

    private static List<int> inputList = new List<int>();
    private static List<int> sortedList = new List<int>();

    private static void M1(object sender, Tuple<int, int, int?> tuple)
    {
        Debug.WriteLine($"M1| {((AlgorithmBase<int>)sender).GetType()}|Thread id: {tuple.Item3}");
    }

    static SortTests()
    {
        Init();
    }

    private static void Init()
    {
        for (int i = 0; i < 10000; i++)
        {
            inputList.Add(rnd.Next(0, 1000));
        }

        sortedList.AddRange(inputList.OrderBy(x => x).ToArray());
        Debug.WriteLine($"Elemets: {inputList.Count / 1000}k");
    }

    [Fact(DisplayName = "StatInsertion")]
    public void SortingStatisticsTest()
    {
        // arrange
        var sortAlgorithm1 = new InsertionSort<int>(inputList);

        // act
        var stat1 = new SortingStatistics<int>(sortAlgorithm1, 100, 10);
        stat1.StartCollectingStatistics();

        Debug.WriteLine(stat1.GetStatistics().ToString());
        Debug.WriteLine($"Better threads count: {stat1.BetterThreadsCountOfTime()}");

        // assert
        Assert.True(true);
    }

    [Fact(DisplayName = ".NET Sort")]
    public void StandartSort()
    {
        // arrange
        List<int> myCollection = new List<int>();
        myCollection.AddRange(inputList);

        // act
        myCollection.Sort();

        // assert
        myCollection.Should().BeEquivalentTo(sortedList);
    }

    [Fact(DisplayName = "MergeSortAlgorithm")]
    public void MergeSortTest()
    {
        // arrange
        var sortAlgorithm = new MergeSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;

        // act
        sortAlgorithm.StartSort();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }
    
    [Fact(DisplayName = "MergeSortAlgorithm")]
    public void Merge2ThreadingSortTest()
    {
        // arrange
        var sortAlgorithm = new MergeSort<int>(inputList);
        sortAlgorithm.Threads = 2;
        //sortAlgorithm.CompareEvent += M1;

        // act
        sortAlgorithm.StartMultiThreadingSort().Wait();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "BubbleSortAlgorithm")]
    public void BubbleSortTest()
    {
        // arrange
        var sortAlgorithm = new BubbleSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;

        // act
        sortAlgorithm.StartSort();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "BubbleSortAlgorithm")]
    public void Bubble100ThreadingSortTest()
    {
        // arrange
        var sortAlgorithm = new BubbleSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;
        sortAlgorithm.Threads = 100; //100 Потоков давали максимальный прирост производительности на 20к

        // act
        sortAlgorithm.StartMultiThreadingSort().Wait();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "QuickSortAlgorithm")]
    public void QuickSortTest()
    {
        // arrange
        var sortAlgorithm = new QuickSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;

        // act
        sortAlgorithm.StartSort();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "QuickSortAlgorithm")]
    public void Quick2ThreadingSortTest()
    {
        // arrange
        var sortAlgorithm = new QuickSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;
        sortAlgorithm.Threads = 2;

        // act
        sortAlgorithm.StartMultiThreadingSort().Wait();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "SelectionSortAlgorithm")]
    public void SelectionSortTest()
    {
        // arrange
        var sortAlgorithm = new SelectionSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;

        // act
        sortAlgorithm.StartSort();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "SelectionSortAlgorithm")]
    public void Selection100ThreadingSortTest()
    {
        // arrange
        var sortAlgorithm = new SelectionSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;
        sortAlgorithm.Threads = 100;

        // act
        sortAlgorithm.StartMultiThreadingSort().Wait();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "InsertionSortAlgorithm")]
    public void InsertionSortTest()
    {
        // arrange
        var sortAlgorithm = new InsertionSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;

        // act
        sortAlgorithm.StartSort();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "InsertionSortAlgorithm")]
    public void Insertion100ThreadingSortTest()
    {
        // arrange
        var sortAlgorithm = new InsertionSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;
        sortAlgorithm.Threads = 100;

        // act
        sortAlgorithm.StartMultiThreadingSort().Wait();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "BinaryInsertionSortAlgorithm")]
    public void BinaryInsertion100ThreadingSortTest()
    {
        // arrange
        var sortAlgorithm = new BinaryInsertionSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;
        sortAlgorithm.Threads = 100;

        // act
        sortAlgorithm.StartMultiThreadingSort().Wait();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "BinaryInsertionSortAlgorithm")]
    public void BinaryInsertionSortTest()
    {
        // arrange
        var sortAlgorithm = new BinaryInsertionSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;

        // act
        sortAlgorithm.StartSort();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "ShellSortAlgorithm")]
    public void ShellSortTest()
    {
        // arrange
        var sortAlgorithm = new ShellSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;

        // act
        sortAlgorithm.StartSort();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }
    
    [Fact(DisplayName = "ShellSortAlgorithm")]
    public void Shell10ThreadingSortTest()
    {
        // arrange
        var sortAlgorithm = new ShellSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;
        sortAlgorithm.Threads = 10;

        // act
        sortAlgorithm.StartMultiThreadingSort().Wait();


        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "CocktailSortAlgorithm")]
    public void CocktailSortTest()
    {
        // arrange
        var sortAlgorithm = new CocktailSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;

        // act
        sortAlgorithm.StartSort();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }

    [Fact(DisplayName = "CocktailSortAlgorithm")]
    public void Cocktail100ThreadingSortTest()
    {
        // arrange
        var sortAlgorithm = new CocktailSort<int>(inputList);
        //sortAlgorithm.CompareEvent += M1;
        sortAlgorithm.Threads = 100;

        // act
        sortAlgorithm.StartMultiThreadingSort().Wait();

        // assert
        sortedList.Should().BeEquivalentTo(sortAlgorithm.Items);
    }
}