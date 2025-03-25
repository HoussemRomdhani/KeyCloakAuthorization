using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Authorization;

public class MustOwnBookHandler : AuthorizationHandler<MustOwnBookRequirement>
{
    private readonly BookService bookService;
    private readonly IHttpContextAccessor httpContextAccessor;

    public MustOwnBookHandler(BookService bookService, IHttpContextAccessor httpContextAccessor)
    {
        this.bookService = bookService;
        this.httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,  MustOwnBookRequirement requirement)
    {
        var id = httpContextAccessor.HttpContext?.GetRouteValue("id")?.ToString();

        if (!int.TryParse(id, out int bookId))
        {
            context.Fail();
            return;
        }

        Book? book = bookService.GetById(bookId);

        if (book == null)
        {
            context.Fail();
            return;
        }

        if ((context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) == null ||
           !int.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            context.Fail();
            return;
        }

        if (book.AuthorId != userId)
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);

        await Task.CompletedTask;
    }
}
