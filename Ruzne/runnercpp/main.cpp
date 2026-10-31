// g++ -std=c++23 -o runner main.cpp
// ./runner

//#define ASSIGNMENT_REPORT

#include <iostream>
#include <streambuf>
#include <string>
#include <cstdio>
#include <unistd.h>     // dup, dup2, close
#include <fcntl.h>      // open
#include <type_traits>
#include <functional>
#include <vector>
#include <array>
#include <optional>
#include <numeric>
#include <ranges>
#include <print>
#include <algorithm>

using namespace std;

int actualAnalogWritePin = 0;
int inputAnalogWriteValue = 0;

int analogRead(int pin)
{
    actualAnalogWritePin = pin;
    return inputAnalogWriteValue;
}

namespace Submission
{
    #include "solution.h"
}

namespace Reference
{
    struct AnalogInput
    {
    private:
        int pin;
    public:
        AnalogInput(int pin) : pin(pin)
        {
            
        }

        int read()
        {
            return analogRead(pin);
        }
    };

    void KlientskyKod()
    {
        AnalogInput analogInput(12);

        int value = analogInput.read();
    }
}

struct TestResult
{ 
    const string description;
    const string expected;
    const string actual;
    const bool isCorrect;
    const int points;

    TestResult(const string description, const string expected, const string actual, bool isCorrect, int points)
        : description(description), expected(expected), actual(actual), isCorrect(isCorrect), points(points)
    {
        
    }
};

using TestFunctionDelegate = function<TestResult(const vector<reference_wrapper<const TestResult>>&)>;

class TestCase
{
private:
    optional<TestResult> cachedResult = nullopt;
    const TestFunctionDelegate testFunction;
public:

    const int maxPoints;
    const vector<reference_wrapper<const TestResult>> dependencies;

    TestCase(const int maxPoints, const vector<reference_wrapper<const TestResult>> dependencies, const TestFunctionDelegate testFunction)
        : maxPoints(maxPoints), dependencies(dependencies), testFunction(testFunction)
    {
        
    }

    const TestResult& result()
    {
        if (!cachedResult.has_value())
        {
            cachedResult.emplace(testFunction(dependencies));
        }

        return cachedResult.value();
    }
};

template<typename T>
concept HasValidConstructor = requires(int pin) {
    T(pin); 
};

template<typename T>
concept HasRead = requires {
    static_cast<int (T::*)()>(&T::read);
};

template<typename T>
concept HasNotPublicPin = !requires(T& object) {
    object.pin;
};

template<typename T>
string testAnalogInput(int pin, int value) 
{
    if constexpr (HasValidConstructor<T> && HasRead<T>) {
        T test(pin);

        inputAnalogWriteValue = value;

        int readValue = test.read();

        return format("pin: {}, read: {}", actualAnalogWritePin, readValue);
    }

    return "Cannot test AnalogInput: missing required constructor or read() method.";
}

TestCase TestRun(const int pin, const int inputValue, const vector<reference_wrapper<const TestResult>>& dependenciesParametr) 
{
    auto testCase = TestCase(
        1,
        dependenciesParametr,
        [pin, inputValue](const vector<reference_wrapper<const TestResult>>& dependencies) {

            const string description = format("Test run, pin: {}, value: {}", pin, inputValue);
            const string expected = testAnalogInput<Reference::AnalogInput>(pin, inputValue);

            bool allCorrect = ranges::all_of(dependencies, [](const auto& dep) { return dep.get().isCorrect; });

            if (!allCorrect)
            {
                return TestResult(description, expected, "nebyly splněné všechny podmínky", false, 0);
            }

            const string actual = testAnalogInput<Submission::AnalogInput>(pin, inputValue);
            
            if(actual == expected)
            {
                return TestResult(description, expected, actual, true, 1);
            }
            else
            {
                return TestResult(description, expected, actual, false, 0);
            }
        }
    );

    return testCase;
}

int main() {
    // --------------------------------------------------
    // 1. Mute std::cout
    // --------------------------------------------------

    std::streambuf* oldCout = std::cout.rdbuf();
    std::cout.rdbuf(nullptr);

    // --------------------------------------------------
    // 2. Mute printf / stdout
    // --------------------------------------------------
    const int oldStdout = dup(STDOUT_FILENO);

    const int nullFd = open("/dev/null", O_WRONLY);
    dup2(nullFd, STDOUT_FILENO);
    close(nullFd);

    // --------------------------------------------------
    // 3. Test studenta
    // --------------------------------------------------

    TestCase tc1(
        1,
        {},
        [](const vector<reference_wrapper<const TestResult>>& dependencies) {
            
            string description = "Definovaná struktura AnalogInput";
            string expected = "ano";

            constexpr bool valid = is_class_v<Submission::AnalogInput>;
            
            if(valid)
            {
                return TestResult(description, expected, expected, true, 1);
            }
            else
            {
                return TestResult(description, expected, "ne", false, 0);
            }
        }
    );

    TestCase tc2(
        1,
        {},
        [](const vector<reference_wrapper<const TestResult>>& dependencies) {
            
            string description = "Definovaná funkce read()";
            string expected = "ano";

            constexpr bool valid = HasRead<Submission::AnalogInput>;
            
            if(valid)
            {
                return TestResult(description, expected, expected, true, 1);
            }
            else
            {
                return TestResult(description, expected, "ne", false, 0);
            }
        }
    );

    TestCase tc3(
        1,
        {},
        [](const vector<reference_wrapper<const TestResult>>& dependencies) {

            const string description = "Spravný konstruktor AnalogInput";
            const string expected = "ano";

            const bool valid = HasValidConstructor<Submission::AnalogInput>;
            
            if(valid)
            {
                return TestResult(description, expected, expected, true, 1);
            }
            else
            {
                return TestResult(description, expected, "ne", false, 0);
            }
        }
    );

    TestCase tc4(
        1,
        {},
        [](const vector<reference_wrapper<const TestResult>>& dependencies) {

            const string description = "Nemá veřejný atribut pin";
            const string expected = "ano";

            const bool valid = HasNotPublicPin<Submission::AnalogInput>;
            
            if(valid)
            {
                return TestResult(description, expected, expected, true, 1);
            }
            else
            {
                return TestResult(description, expected, "ne", false, 0);
            }
        }
    );

    const vector<reference_wrapper<const TestResult>>& dependencies = { cref(tc1.result()), cref(tc2.result()), cref(tc3.result()) };

    TestCase tc5 = TestRun(1, 6, dependencies);
    TestCase tc6 = TestRun(2, 128, dependencies);

    auto testCases = array{ ref(tc1), ref(tc2), ref(tc3), ref(tc4), ref(tc5), ref(tc6) };

    int maxPoints = 0;
    int points = 0;

    for (const auto& testCaseReference : testCases)
    {
        auto& testCase = testCaseReference.get();
        const auto& result = testCase.result();

        maxPoints += testCase.maxPoints;
        points += result.points;
    }
    
    double fraction = maxPoints > 0 ? static_cast<double>(points) / maxPoints : 0.0;

    // --------------------------------------------------
    // 4. Restore outputs
    // --------------------------------------------------

    std::cout.rdbuf(oldCout);

    fflush(stdout);
    dup2(oldStdout, STDOUT_FILENO);
    close(oldStdout);

    // --------------------------------------------------
    // 5. Print results
    // --------------------------------------------------

    #ifdef ASSIGNMENT_REPORT

    println("Výsledné body se spočítají jako: (počet bodů za splněné kritéria / {}) x celkový počet bodů za úlohu.", maxPoints);

    int i = 1;

    for (const auto& testCaseReference : testCases)
    {
        auto& testCase = testCaseReference.get();
        const auto& result = testCase.result();
    
        print("{}. {} (body: {}).", i, result.description, testCase.maxPoints);

        if(testCase.dependencies.size() > 0)
        {
            print(" [Závisí na: ");

            print("{}", testCase.dependencies.front().get().description);

            for (auto& dependency : testCase.dependencies | std::views::drop(1))
            {
                print(", {}", dependency.get().description);
            }

            print("]");
        }

        println();
        

        ++i;
    }

    #endif
    
    print(R"({{ "fraction": {}, "testresults":  [)", fraction);
    print(R"( ["Název testu", "Očekávaná hodnota", "Výsledek", "iscorrect"])");

    for (const auto& testCaseReference : testCases)
    {
        auto& testCase = testCaseReference.get();
        const auto& result = testCase.result();
  
        print(R"(, ["{}", "{}", "{}", {}])", result.description, result.expected, result.actual, result.isCorrect ? 1 : 0);
    }

    print(R"( ]}})");

    return 0;
}
