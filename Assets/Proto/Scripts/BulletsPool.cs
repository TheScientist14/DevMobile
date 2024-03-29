namespace Proto
{
public class BulletsPool : PoolBase<BulletTest>
{
    public static BulletsPool SharedInstance;

    private void Awake()
    {
        if (SharedInstance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            SharedInstance = this;
        }
    }

}
}
