// Visual studio 2022

await VypisVtipAsync();


async Task VypisVtipAsync()
{
    using (HttpClient client = new HttpClient())
    {
        string text = await client.GetStringAsync("https://geek-jokes.sameerkumar.website/api?format=plain");
        Console.WriteLine(text);
    }
}

async Task<string> NactiVtipAsync()
{
    using (HttpClient client = new HttpClient())
    {
        string text = await client.GetStringAsync("https://geek-jokes.sameerkumar.website/api?format=plain");
        return text;
    }
}
