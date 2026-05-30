#include <SDL3/SDL.h>
#include <iostream>
#include <vector>
#include <cmath>
#include <memory>

// Globální konstanty pro simulaci
const int SCREEN_WIDTH = 800;
const int SCREEN_HEIGHT = 600;
const float PI = 3.14159265f;

// Pomocná struktura pro překážku (Zapouzdření geometrie)
struct Obstacle {
    SDL_FRect rect;
};

// --- KOMPOZICE: Třída reprezentující ultrazvukový senzor ---
class UltrasonicSensor {
private:
    float maxDistance; // Maximální dosah senzoru v pixelech
public:
    UltrasonicSensor(float distance) : maxDistance(distance) {}

    // Metoda simuluje odraz paprsku od překážek a vrací změřenou vzdálenost
    float measureDistance(float robotX, float robotY, float angle, const std::vector<Obstacle>& obstacles, float& hitX, float& hitY) {
        // Výpočet koncového bodu senzoru, pokud nic netrefí
        hitX = robotX + cosf(angle) * maxDistance;
        hitY = robotY + sinf(angle) * maxDistance;

        float shortestDistance = maxDistance;

        // Velmi jednoduchý raycasting (vzorkování po pixelech) pro detekci kolize s obdélníky
        // Pro studenty snadno pochopitelné na rozdíl od složité analytické geometrie
        for (int i = 0; i < (int)maxDistance; i += 2) {
            float checkX = robotX + cosf(angle) * (float)i;
            float checkY = robotY + sinf(angle) * (float)i;

            for (const auto& obs : obstacles) {
                // Kontrola, zda bod paprsku leží uvnitř obdélníku překážky
                if (checkX >= obs.rect.x && checkX <= obs.rect.x + obs.rect.w &&
                    checkY >= obs.rect.y && checkY <= obs.rect.y + obs.rect.h) {
                    
                    if ((float)i < shortestDistance) {
                        shortestDistance = (float)i;
                        hitX = checkX;
                        hitY = checkY;
                    }
                }
            }
        }
        return shortestDistance;
    }
};

// --- BÁZOVÁ TŘÍDA: Abstraktní třída Robot (OOP: Zapouzdření, Abstrace) ---
class Robot {
protected:
    float x, y;                // Pozice středu robota
    float radius;              // Velikost robota (poloměr)
    float angle;               // Orientace robota v radiánech (0 = vpravo)
    float speed;               // Rychlost pohybu
    UltrasonicSensor sensor;   // Kompozice: Každý robot MÁ senzor
    SDL_Color color;           // Barva pro vykreslení
    
    // Proměnné pro uložení dat senzoru pro následné vykreslení
    float lastSensorHitX = 0;
    float lastSensorHitY = 0;
    bool obstacleDetected = false;

public:
    Robot(float startX, float startY, float startAngle, float r, float s, float sensorDist, SDL_Color col)
        : x(startX), y(startY), angle(startAngle), radius(r), speed(s), sensor(sensorDist), color(col) {}

    virtual ~Robot() = default; // Virtuální destruktor pro správný polymorfismus

    // Čistě virtuální metoda - definuje chování (strategii) vyhýbání (OOP: Polymorfismus)
    virtual void updateBehavior(float distance) = 0;

    // Aktualizace logiky robota (pohyb a měření)
    void update(const std::vector<Obstacle>& obstacles, float deltaTime) {
        // 1. Senzor změří vzdálenost k překážce
        float distance = sensor.measureDistance(x, y, angle, obstacles, lastSensorHitX, lastSensorHitY);
        
        // Nastavení flagu detekce (např. pokud je překážka blíž než 80 pixelů)
        obstacleDetected = (distance < 80.0f);

        // 2. Volání polymorfního chování podle typu robota
        updateBehavior(distance);

        // 3. Pohyb vpřed na základě aktuálního úhlu a rychlosti
        x += cosf(angle) * speed * deltaTime;
        y += sinf(angle) * speed * deltaTime;

        // Okrajové podmínky (teleportace zpět na obrazovku při vyjetí)
        if (x < 0) x = SCREEN_WIDTH;
        if (x > SCREEN_WIDTH) x = 0;
        if (y < 0) y = SCREEN_HEIGHT;
        if (y > SCREEN_HEIGHT) y = 0;
    }

    // Metoda pro vykreslení robota (používá SDL_RenderFillRect pro zjednodušení)
    void render(SDL_Renderer* renderer) {
        // Vykreslení těla robota jako čtverce reprezentujícího kruh (v SDL3 přímočaré)
        SDL_SetRenderDrawColor(renderer, color.r, color.g, color.b, color.a);
        SDL_FRect robotRect = { x - radius, y - radius, radius * 2, radius * 2 };
        SDL_RenderFillRect(renderer, &robotRect);

        // Vykreslení čáry senzoru (OOP: vizualizace stavu komponenty)
        if (obstacleDetected) {
            SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255); // Červená čára = detekce!
        } else {
            SDL_SetRenderDrawColor(renderer, 0, 255, 0, 255); // Zelená čára = čisto
        }
        SDL_RenderLine(renderer, x, y, lastSensorHitX, lastSensorHitY);
    }
};

// --- ODVOZENÁ TŘÍDA 1: Robot uhýbající doprava (OOP: Dědičnost) ---
class RightTurningRobot : public Robot {
public:
    RightTurningRobot(float x, float y, float angle) 
        : Robot(x, y, angle, 15.0f, 100.0f, 120.0f, {0, 150, 255, 255}) {} // Modrý robot

    void updateBehavior(float distance) override {
        // Pokud je překážka blízko, začne se ostře otáčet doprava
        if (distance < 80.0f) {
            angle += 3.0f * 0.016f; // Otáčení (radiány za snímek)
        }
    }
};

// --- ODVOZENÁ TŘÍDA 2: Robot, který zazmatkuje a začne couvat ---
class ErraticRobot : public Robot {
private:
    bool backingUp = false;
    float backupTimer = 0.0f;
public:
    ErraticRobot(float x, float y, float angle) 
        : Robot(x, y, angle, 15.0f, 120.0f, 100.0f, {255, 165, 0, 255}) {} // Oranžový robot

    void updateBehavior(float distance) override {
        if (backingUp) {
            speed = -60.0f; // Couvání poloviční rychlostí
            angle -= 1.5f * 0.016f; // Mírné otáčení při couvání
            backupTimer -= 0.016f;
            if (backupTimer <= 0) {
                backingUp = false;
                speed = 120.0f; // Návrat k jízdě vpřed
            }
        } else if (distance < 60.0f) {
            backingUp = true;
            backupTimer = 0.8f; // Bude couvat 0.8 sekundy
        }
    }
};

// --- HLAVNÍ PROGRAM ---
int main(int argc, char* argv[]) {
    if (!SDL_Init(SDL_INIT_VIDEO)) {
        std::cerr << "SDL inicializace selhala: " << SDL_GetError() << std::endl;
        return -1;
    }

    SDL_Window* window = SDL_CreateWindow("Robotika OOP Simulace - SDL3", SCREEN_WIDTH, SCREEN_HEIGHT, 0);
    SDL_Renderer* renderer = SDL_CreateRenderer(window, nullptr);

    // Vytvoření světa (překážek)
    std::vector<Obstacle> obstacles = {
        { { 250.0f, 200.0f, 100.0f, 150.0f } },
        { { 550.0f, 100.0f, 120.0f, 120.0f } },
        { { 400.0f, 450.0f, 200.0f, 60.0f } }
    };

    // Vytvoření robotů pomocí polymorfismu (std::unique_ptr a bázový ukazatel)
    std::vector<std::unique_ptr<Robot>> robots;
    robots.push_back(std::make_unique<RightTurningRobot>(100.0f, 100.0f, 0.0f));
    robots.push_back(std::make_unique<ErraticRobot>(100.0f, 400.0f, 0.5f));

    bool running = true;
    Uint64 lastTime = SDL_GetTicks();

    // Hlavní herní smyčka
    while (running) {
        SDL_Event event;
        while (SDL_PollEvent(&event)) {
            if (event.type == SDL_EVENT_QUIT) {
                running = false;
            }
        }

        // Výpočet deltaTime pro plynulý pohyb nezávislý na FPS
        Uint64 currentTime = SDL_GetTicks();
        float deltaTime = (float)(currentTime - lastTime) / 1000.0f;
        lastTime = currentTime;
        
        // Zamezení skokům při re-focusu okna
        if (deltaTime > 0.1f) deltaTime = 0.1f; 

        // UPDATE logiky
        for (auto& robot : robots) {
            robot->update(obstacles, deltaTime);
        }

        // RENDER scény
        SDL_SetRenderDrawColor(renderer, 30, 30, 30, 255); // Tmavé pozadí laborky
        SDL_RenderClear(renderer);

        // Vykreslení překážek (vyplněné obdélníky v SDL3)
        SDL_SetRenderDrawColor(renderer, 120, 120, 120, 255); // Šedé krabice
        for (const auto& obs : obstacles) {
            SDL_RenderFillRect(renderer, &obs.rect);
        }

        // Vykreslení všech robotů přes polymorfní volání
        for (auto& robot : robots) {
            robot->render(renderer);
        }

        SDL_RenderPresent(renderer);
        SDL_Delay(16); // Cílíme zhruba na 60 FPS
    }

    SDL_DestroyRenderer(renderer);
    SDL_DestroyWindow(window);
    SDL_Quit();

    return 0;
}
