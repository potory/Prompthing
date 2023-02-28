using Prompthing.Core.Abstract;

namespace Prompthing.Core.Entities;

public class Value : IWeighted
{
    public string Text { get; }
    public double Weight { get; }

    public Value(string text, double weight)
    {
        Text = text;
        Weight = weight;
    }
}