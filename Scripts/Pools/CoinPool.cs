public class CoinPool : Pool<Coin>
{
    protected override Coin CreateNewObject()
    {
        Coin newObj = Instantiate(Template, Container);

        newObj.Initialize(Container);
        newObj.Deactivate();
        ObjectsPool.Add(newObj);

        return newObj;
    }
}
