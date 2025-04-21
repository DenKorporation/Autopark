namespace Autopark.Common.Extensions;

public static class RequestExtensions
{
    /// <summary>
    /// Получить все данные, запрашивая данные партиями
    /// </summary>
    /// <param name="query">Делегат возвращающий партию данных, в котором 1-ый параметр - это отступ, а 2-ой - размер партии</param>
    /// <param name="batchSize">Размер партии</param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T">Тип данных в коллекции</typeparam>
    /// <returns>Вся коллекция данных</returns>
    public static async Task<IList<T>> GetAllDataAsync<T>(
        Func<int, int, CancellationToken, Task<IList<T>>> query,
        int batchSize = 5000,
        CancellationToken cancellationToken = default)
    {
        IList<T> result = new List<T>();

        var first = 0;
        IList<T> temp;

        do
        {
            temp = await query(first, batchSize, cancellationToken);
            result = result.Concat(temp).ToList();
            first += batchSize;
        } while (temp.Count == batchSize);

        return result;
    }
}
