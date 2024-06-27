# Persist Prerendered State

```razor
@page "/"
@rendermode InteractiveServer

<PageTitle>Counter</PageTitle>

<h1>Counter</h1>

<p role="status">Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    protected override void OnInitialized()
    {
        currentCount = Random.Shared.Next(100);
    }
    private void IncrementCount()
    {
        currentCount++;
    }
}
```

```razor
@page "/"

@implements IDisposable
@rendermode InteractiveServer

@inject PersistentComponentState PersistentComponentState

<PageTitle>Counter</PageTitle>

<h1>Counter</h1>

<p role="status">Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    PersistingComponentStateSubscription persistingComponentStateSubscription;


    protected override void OnInitialized()
    {
        persistingComponentStateSubscription = PersistentComponentState.RegisterOnPersisting(PersistData);

        if (!PersistentComponentState.TryTakeFromJson(nameof(currentCount), out int restored))
        {
            currentCount = Random.Shared.Next(100);
        }
        else
        {
            currentCount = restored;
        }
    }

    private void IncrementCount()
    {
        currentCount++;
    }

    private Task PersistData()
    {
        PersistentComponentState.PersistAsJson(nameof(currentCount), currentCount);

        return Task.CompletedTask;
    }

    void IDisposable.Dispose()
    {
        persistingComponentStateSubscription.Dispose();
    }
}
```