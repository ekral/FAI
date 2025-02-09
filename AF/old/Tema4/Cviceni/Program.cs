using SharedProject;

DatabaseService databaseService = new DatabaseService();

if(await databaseService.EnsureCreatedAsync())
{
    await databaseService.InsertAsync(new Model(8000000.0, 5.7, 30));
    await databaseService.InsertAsync(new Model(4000000.0, 5.8, 20));
    await databaseService.InsertAsync(new Model(10800000.0, 6.2, 15));
}

Model? model = await databaseService.GetByIdAsync(2);

if (model is not null)
{
    model.LoanAmount = 4800000.0;
    model.InterestRate = 5.0;
    model.LoanTerm = 5;
    
    await databaseService.UpdateAsync(model);
}

//await databaseService.DeleteAsync(new Model() { Id = 3 });

List<Model> models = await databaseService.GetAllAsync();

foreach (Model m in models)
{
    Console.WriteLine($"{m.Id, 3} {m.LoanAmount, 16:C1} {m.InterestRate, 4:F1} % {m.LoanTerm, 3}");
}


