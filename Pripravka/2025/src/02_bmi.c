#include <stdio.h>

int main()
{
	// = je operator prirazeni
	double hmotnost = 96.0;
	double vyska = 1.86;

	double bmi = hmotnost / (vyska * vyska);

	printf("vyska: %.2lF cm\n", vyska * 100);
	printf("hmotnost: %.2lF kg\n", hmotnost);
	printf("bmi: %.2lF\n", bmi);

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

	// - je operator odcitani
	double rozdil = hmotnost - max_hmotnost;

	printf("Mel by jsi zhubnout: %.2lF\n", rozdil);

	return 0;
}
