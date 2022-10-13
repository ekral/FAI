using SharedProject;

DatabaseService service = new DatabaseService();
if (await service.EnsureCreatedAsync())
{
    await service.InsertAsync(new Model(8000000.0, 6.0, 30));
    await service.InsertAsync(new Model(4000000.0, 5.0, 20));
    await service.InsertAsync(new Model(10000000.0, 6.0, 30));
}

List<Model> models = await service.GetAll();

foreach(Model model in models)
{
    Console.WriteLine($"{model.Id} {model.LoanAmount, 18:C1} {model.InterestRate, 4:F1} {model.LoanTerm, 3}");
}
