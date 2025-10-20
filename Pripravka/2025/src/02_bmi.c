#include <stdio.h>

int main()
{
	double vyska_cm = 0.0;

	printf("Zadej vysku v cm: ");
	scanf_s("%lF", &vyska_cm);

	double vyska = vyska_cm / 100.0;

	double hmotnost = 0.0;

	printf("Zadej hmotnost v kg: ");
	scanf_s("%lF", &hmotnost);

	double bmi = hmotnost / (vyska * vyska);

	puts("=============================");

	printf("vyska: %.2lF cm\n", vyska * 100);
	printf("hmotnost: %.2lF kg\n", hmotnost);
	printf("bmi: %.2lF\n", bmi);

	// kategorie

	if (bmi > 40.0)
	{
		puts("morbidni obezita");
	}
	else if (bmi > 35.0)
	{
		puts("obezita druheho stupne");
	}
	else if (bmi > 30.0)
	{
		puts("obezita prvniho stupne");
	}
	else if (bmi > 25.0)
	{
		puts("nadvaha");
	}
	else if (bmi > 18.5)
	{
		puts("zdrava vaha");
	}
	else if (bmi > 16.5)
	{
		puts("podvaha");
	}
	else
	{
		puts("tezka podvyziva");
	}

	// spocitejte nejmensi a nejvetsi doporucenou
	// doporucenou hmotnost dle vysky

	double min_bmi = 18.5;
	double min_hmotnost = min_bmi * (vyska * vyska);

	printf("minimalni zdrava hmotnost: %.0lF\n", min_hmotnost);

	// spocitejte a vypiste maximalni zdravou hmostnost

	double max_bmi = 25.0;
	double max_hmotnost = max_bmi * (vyska * vyska);

	printf("maximalni zdrava hmotnost: %.0lF\n", max_hmotnost);

	// vypiste o kolik kilogramu by mela osoba zhubnout
	// pokud vazi mene tak vypise zaporne cislo

	// Pokud je hmotnost vetsi nez maximalni zdrava hmotnost, tak vypise kolik ma zhubnout
	// Pokud je hmotnost mensi nez minimalni zdrava hmotnost, tak napise kolik ma pribrat
	// Jinak napise: "gratuluji mas idealni vahu".

	if (hmotnost > max_hmotnost)
	{
		double rozdil = hmotnost - max_hmotnost;
		printf("Mel by jsi zhubnout %.2lF kg\n", rozdil);
	}
	else if (hmotnost < min_hmotnost)
	{
		double rozdil = min_hmotnost - hmotnost;
		printf("Mel by jsi pribrat %.2lF kg\n", rozdil);
	}
	else
	{
		puts("gratuluji mas idealni vahu");
	}


	return 0;
}
