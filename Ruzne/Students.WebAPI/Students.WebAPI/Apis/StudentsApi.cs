namespace Students.WebAPI.Apis
{
    public static class StudentsApi 
    {
        public static IEndpointRouteBuilder MapStudentsApi(this IEndpointRouteBuilder app)
        {
            app.MapPost("/seed", WebApiVersion1.Seed);

            var studentItems = app.MapGroup("/students");

            studentItems.MapGet("/", WebApiVersion1.GetAllStudents);
            studentItems.MapGet("/active", WebApiVersion1.GetActiveStudents);
            studentItems.MapGet("/{id}", WebApiVersion1.GetStudent);
            studentItems.MapPost("/", WebApiVersion1.CreateStudent);
            studentItems.MapPut("/{id}", WebApiVersion1.UpdateStudent);
            studentItems.MapDelete("/{id}", WebApiVersion1.DeleteStudent);

            return app;
        }
    }
}
