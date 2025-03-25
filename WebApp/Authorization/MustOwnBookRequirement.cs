using Microsoft.AspNetCore.Authorization;

namespace WebApp.Authorization;

public class MustOwnBookRequirement : IAuthorizationRequirement
{
    public MustOwnBookRequirement()
    {
    }
}

