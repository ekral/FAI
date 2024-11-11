#include <iostream>
#include <string>
#include <vector>

using namespace std;

class Student
{
public:
    string name;
    int age;

    Student(const string &name, const int age) : name(name), age(age)
    {
    }

    ~Student()
    {
        cout << "destroying student" << endl;
    }

    void Print() const
    {
        cout << "name: " << name << endl;
        cout << "age: " << age << endl;
    }
};

int main() {

    vector<Student*> students;

    bool end = false;

    do {
        cout << "Menu" << endl;
        cout << "1 Add new student." << endl;
        cout << "2 Print all students." << endl;
        cout << "3 Exit." << endl;

        char choice = 0;
        cin >> choice;

        switch (choice)
        {
            case '1':
            {
                cout << "Enter student name:" << endl;
                string name;
                cin >> name;

                cout << "Enter student age:" << endl;
                int age;
                cin >> age;

                Student* pStudent = new Student(name, age);

                students.push_back(pStudent);
            }
                break;
            case '2':
                for (const Student* pStudent : students)
                {
                    pStudent->Print();
                }
                break;
            case '3':
                end = true;
                break;
        }

    } while (end == false);

    for (const Student* pStudent : students)
    {
        delete pStudent;
    }

    students.clear();

    return 0;
}
