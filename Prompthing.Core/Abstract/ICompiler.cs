namespace Prompthing.Core.Abstract;

public interface ICompiler<in TSource, out TEntity>
{
    public TEntity Compile(TSource obj);
}