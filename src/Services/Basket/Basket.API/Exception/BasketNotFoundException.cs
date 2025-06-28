namespace Basket.API.Exception
{
    public class BasketNotFoundException(string userName) : NotFoundException("Basket", userName);
}
