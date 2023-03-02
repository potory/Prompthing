using Prompthing.Core.Abstract;

namespace Prompthing.Core.Extensions;

public static class ArrayExtensions
{
    public static T OneOfWeighted<T>(this IReadOnlyCollection<T> collection) where T: IWeighted
    {
        var totalWeight = collection.Sum(item => item.Weight);
        var randomValue = Random.Shared.NextDouble() * totalWeight;

        foreach (var item in collection)
        {
            if (randomValue < item.Weight)
            {
                return item;
            }
            randomValue -= item.Weight;
        }

        throw new InvalidOperationException("The weighted items list was empty or all items had zero weight.");
    }
}