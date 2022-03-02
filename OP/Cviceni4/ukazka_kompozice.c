struct osoba
{
	char* jmeno;
	char* prijmeni;
};

// kompozice
struct student
{
	struct osoba osoba;
	char* skupina;
};

struct zamestnanec
{
	struct osoba osoba;
	char* kancelar;
};

void osoba_init(struct osoba* osoba, char* jmeno, char* prijmeni)
{
	osoba->jmeno = jmeno;
	osoba->prijmeni = prijmeni;
}

void student_init(struct student* student, char* jmeno, char* prijmeni, char* skupina)
{
	osoba_init(&student->osoba, jmeno, prijmeni);
	student->skupina = skupina;
}

int main()
{
	struct student student1;
	student_init(&student1, "Michal", "Vesely", "AXP1");

	struct zamestnanec zamestnanec1;
	zamestnanec1.osoba.jmeno = "Peppa";
	zamestnanec1.osoba.prijmeni = "Prasatko";
	zamestnanec1.kancelar = "822";

	// V jazyce C muzu pozit inicializaci
	struct student student2 = { .osoba.jmeno = "Michal", .osoba.prijmeni = "Vesely", .skupina = "AXP1" };
}
