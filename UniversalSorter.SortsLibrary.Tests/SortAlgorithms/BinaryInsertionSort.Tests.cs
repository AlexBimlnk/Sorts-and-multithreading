using UniversalSorter.SortsLibrary.SortAlgorithms;

namespace UniversalSorter.SortsLibrary.Tests.SortAlgorithms;

public class BinaryInsertionSortTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("BinaryInsertionSort", "Unit")]
    public void CanCreate()
    {
        // Arrange
        var items = new int[3];
        var threads = 3;

        // Act
        var exception = Record.Exception(() => new BinaryInsertionSort<int>(items, threads));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Can be created without count threads.")]
    [Trait("Category", "Unit")]
    public void CanCreateWithoutCountThreads()
    {
        // Arrange
        var items = new int[3];

        // Act
        var exception = Record.Exception(() => new BinaryInsertionSort<int>(items));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Can not be created when items in null.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWhenItemsIsNull()
    {
        // Act
        var exception = Record.Exception(() => new BinaryInsertionSort<int>(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [InlineData(-1)]
    [InlineData(0)]
    [Theory(DisplayName = "Can not be created when threads count is incorrent.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWhenThreadsCountIsInvalid(int threads)
    {
        // Arrange
        var items = new int[3];

        // Act
        var exception = Record.Exception(() => new BinaryInsertionSort<int>(items, threads));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(100)]
    [Theory(DisplayName = "Can set count threads.")]
    [Trait("Category", "Unit")]
    public void CanSetCountThreads(int threads)
    {
        // Arrange
        var items = new int[3];
        var algorithm = new BinaryInsertionSort<int>(items);

        // Act
        var exception = Record.Exception(() => algorithm.Threads = threads);

        // Assert
        exception.Should().BeNull();
        algorithm.Threads.Should().Be(threads);
    }

    [InlineData(-1)]
    [InlineData(0)]
    [Theory(DisplayName = "Cannot set invalid count threads.")]
    [Trait("Category", "Unit")]
    public void CanNotSetInvalidCountThreads(int threads)
    {
        // Arrange
        var items = new int[3];
        var algorithm = new BinaryInsertionSort<int>(items);

        // Act
        var exception = Record.Exception(() => algorithm.Threads = threads);

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }
}