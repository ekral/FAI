using(MojeTrida moje = new MojeTrida())
{

}

class MojeTrida : IDisposable
{
    bool uvolneno = false;

    public void Dispose()
    {
        Dispose(uvolniManaged: true);
        GC.SuppressFinalize(this);
    }

    // finalizer (destructor)
    protected void Dispose(bool uvolniManaged)
    {
        if(!uvolneno)
        {
            if(uvolniManaged)
            {
                // zavolam Dispose na objektech ktere to podporuji
            }

            // uvolnim unmanaged resources
            Console.WriteLine("uvolneno");
            uvolneno = true;
        }
    }

    ~MojeTrida()
    {
        // uz pracuje garbage collector
        Dispose(uvolniManaged: false);
    }
}
