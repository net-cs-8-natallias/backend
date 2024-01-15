using Infrastructure.RateLimit.services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Host.Services;

public class FilterAttribute: ServiceFilterAttribute
{
    public FilterAttribute() : base(typeof(IFilterService))
    {
        
    }
}