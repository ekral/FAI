using System.Net.Http.Json;

namespace Knihovna.BlazorApp.Pages
{
    public partial class Home
    {
        public class Vypujcka
        {
            public int VypujckaId { get; set; }
            public int KnihaId { get; set; }
            public int CtenarId { get; set; }
            public DateOnly DatumVypujcky { get; set; }
            public DateOnly? DatumVraceni { get; set; }
        }

        public class Kniha
        {
            public int KnihaId { get; set; }
            public required string Nazev { get; set; }
            public Vypujcka? Vypujcka { get; set; }
        }

        private Kniha[]? knihy;
        private IQueryable<Kniha>? Knihy { get => knihy?.AsQueryable(); }

        protected override async Task OnInitializedAsync()
        {
            HttpResponseMessage response = await Client.GetAsync("/books");

            if(response.IsSuccessStatusCode)
            {
                knihy = await response.Content.ReadFromJsonAsync<Kniha[]>();
            }
        }

    }
}