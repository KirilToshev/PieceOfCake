using System.Linq.Expressions;
using CSharpFunctionalExtensions;

namespace PieceOfCake.WebApi;

public static class Extensions
{
    public static Microsoft.AspNetCore.Http.IResult ConvertToHttpResult<T, E>
        (this Result<E> result, Expression<Func<E, T>> mappingFunction)
    {
        if(result.IsFailure)
            return Results.BadRequest(result.Error);

        var mappedDto = mappingFunction.Compile().Invoke(result.Value);
        return Results.Ok(mappedDto);
    }

    public static Microsoft.AspNetCore.Http.IResult ConvertToHttpResult(this Result result)
    {
        if(result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok();
    }
}
