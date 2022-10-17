using SharedProject;

DatabaseService databaseService = new DatabaseService();

if(await databaseService.EnsureCreatedAsync())
{
    await databaseService.InsertAsync(new Model(8000000.0, 6.0, 30));
    await databaseService.InsertAsync(new Model(10800000.0, 5.8, 30));
    await databaseService.InsertAsync(new Model(4000000.0, 6.2, 20));
}

await databaseService.UpdateAsync(new Model(8800000, 5.9, 25) { Id = 1 });

List<Model> models = await databaseService.GetAll();

foreach (Model m in models)
{
    Console.WriteLine($"{m.Id, 3} {m.LoanAmount, 16:C1} {m.InterestRate, 4:F1} {m.LoanTerm, 3}");
}
