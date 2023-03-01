using Prompthing.Core.Abstract;

namespace Prompthing.Core.Entities;

public class Term : IWeighted
{
    public double Weight { get; }
    public string Text { get; }

    public Term(string text, double weight)
    {
        Text = text;
        Weight = weight;
    }
}