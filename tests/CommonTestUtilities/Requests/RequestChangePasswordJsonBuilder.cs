using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build(int? length = null)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(request => request.CurrentPassword, f => f.Internet.Password())
            .RuleFor(request => request.NewPassword, f => length.HasValue
                                                    ? f.Internet.Password(length: length.Value)
                                                    : f.Internet.Password());
    }
}
