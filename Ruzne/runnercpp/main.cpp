// g++ -std=c++23 -o runner main.cpp
// ./runner

#define ASSIGNMENT_REPORT

#include <iostream>
#include <streambuf>
#include <string>
#include <sstream>
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
#include <format>
#include <string_view>

using namespace std;

bool watch = false;
bool newCalled = false;
bool deleteCalledAfterNew = false;

void* operator new(std::size_t size)
{
    if(watch)
    {
        newCalled = true;
    }

    void* ptr = std::malloc(size);
    
    if (!ptr) {
        throw std::bad_alloc(); // C++ standard vyžaduje výjimku při selhání
    }

    return ptr;
}

void operator delete(void* ptr) noexcept
{
    if(watch && newCalled)
    {
        deleteCalledAfterNew = true;
    }

    std::free(ptr);
}

int result_score = 0;
int result_health = 0;

template<typename T>
concept HasValidConstructor = requires(int health, int score) {
    T(health, score); 
};

template<typename T>
concept HasPublicHealth = requires(T& object) {
    object.health;
};

template<typename T>
concept HasPublicScore = requires(T& object) {
    object.score;
};

template<typename T>
void print(const T* player)
{
    if constexpr (HasPublicHealth<T> && HasPublicScore<T>) 
    {
        int score = player->score;
        int health = player->health;

        result_score = score;
        result_health = health;
    }
}

namespace Submission
{
    #include "solution.h"
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

TestCase TestRun(const int points, const int health, const int score, const vector<reference_wrapper<const TestResult>>& dependenciesParametr) 
{
    auto testCase = TestCase(
        points,
        dependenciesParametr,
        [points,health, score](const vector<reference_wrapper<const TestResult>>& dependencies) {

            const string description = format("Test run, health: {}, score: {}", health, score);

            static constexpr string_view formatString = "Health: {}, Score: {}, New: {}, Delete: {}"sv;

            string expected = format(formatString, health, score, true, true); 

            bool allCorrect = ranges::all_of(dependencies, [](const auto& dep) { return dep.get().isCorrect; });

            if (!allCorrect)
            {
                return TestResult(description, "ano", "nebyly splněné všechny podmínky", false, 0);
            }
            
            if constexpr (HasValidConstructor<Submission::Player> && HasPublicHealth<Submission::Player> && HasPublicScore<Submission::Player>) 
            {
                watch = true;
                newCalled = false;
                deleteCalledAfterNew = false;

                Submission::setup(health, score);

                watch = false;
            }
            
            string actual = format(formatString, result_health, result_score, newCalled, deleteCalledAfterNew);  

            if(actual == expected)
            {
                return TestResult(description, expected, actual, true, points);
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
            
            string description = "Definovaná struktura Player";
            string expected = "ano";

            constexpr bool valid = is_class_v<Submission::Player>;
            
            if(!valid)
            {
                return TestResult(description, expected, "ne", false, 0);
            }

            return TestResult(description, expected, expected, true, 1);
        }
    );

    TestCase tc2(
        1,
        {},
        [](const vector<reference_wrapper<const TestResult>>& dependencies) {

            const string description = "Spravný konstruktor";
            const string expected = "ano";

            const bool valid = HasValidConstructor<Submission::Player>;
            
            if(!valid)
            {
                return TestResult(description, expected, "ne", false, 0);
            }
            
            return TestResult(description, expected, expected, true, 1);
        }
    );

    TestCase tc3(
        1,
        {},
        [](const vector<reference_wrapper<const TestResult>>& dependencies) {

            const string description = "Nemá veřejný atribut health";
            const string expected = "ano";

            const bool valid = HasPublicHealth<Submission::Player>;
            
            if(!valid)
            {
                return TestResult(description, expected, "ne", false, 0);
            }

            return TestResult(description, expected, expected, true, 1);
        }
    );

    TestCase tc4(
        1,
        {},
        [](const vector<reference_wrapper<const TestResult>>& dependencies) {

            const string description = "Nemá veřejný atribut score";
            const string expected = "ano";

            const bool valid = HasPublicScore<Submission::Player>;
            
            if(!valid)
            {
                return TestResult(description, expected, "ne", false, 0);
            }

            return TestResult(description, expected, expected, true, 1);
        }
    );

    const vector<reference_wrapper<const TestResult>>& dependencies = { cref(tc1.result()), cref(tc2.result()), cref(tc3.result()) };

    TestCase tc5 = TestRun(2, 1, 6, dependencies);
    TestCase tc6 = TestRun(2, 2, 128, dependencies);

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
    
    print(R"({{ "fraction": {}, "testresults":  [ )", fraction);

    puts(R"( ["Název testu", "Očekávaná hodnota", "Výsledek", "iscorrect"] )");

    for (const auto& testCaseReference : testCases)
    {
        auto& testCase = testCaseReference.get();
        const auto& result = testCase.result();
  
        print(R"(, ["{}", "{}", "{}", {}])", result.description, result.expected, result.actual, result.isCorrect ? 1 : 0);
    }

    puts(R"( ]} )");

    return 0;
}
