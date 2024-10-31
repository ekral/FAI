#include <iostream>
#include <string>
#include <vector>

using namespace std;

class Student
{
public:
    string name;
    int age;

    Student(const string& name, const int age) : name(name), age(age)
    {  }

    void print() const
    {
        cout << "Student Name: " << name << endl;
        cout << "Student Age: " << age << endl;
    }
};

int main()
{
    vector<Student*> students;

    bool end = false;

    do {
        cout << "Enter choice:" << endl;
        cout << "1 Add new student." << endl;
        cout << "2 Show all students." << endl;
        cout << "3 End program." << endl;

        int choice = 0;
        cin >> choice;

        switch (choice) {
            case 1:
                {
                    cout << "Enter name:" << endl;
                    string name;
                    cin >> name;

                    cout << "Enter age:" << endl;
                    int age;
                    cin >> age;

                    Student* student = new Student(name, age);

                    students.push_back(student);
                }
                break;
            case 2:
                for(const Student* student : students)
                {
                    student->print();
                }
                break;
            case 3:
                for(const Student* student : students)
                {
                    delete student;
                }

                students.clear();

                end = true;
                break;
        }

    } while (end == false);

    cout << "program over." << endl;
    return 0;
}
