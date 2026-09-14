using System.Collections;

namespace WebApi.Tests.InlineData;

public class CultureInlineData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "en-US" }; //yield retorna um valor por vez pro teste
        yield return new object[] { "pt-BR" };

    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
