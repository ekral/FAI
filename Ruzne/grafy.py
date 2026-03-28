k = 0.1      # Tuhost (tu "zadáváš" ty)
m = 1.0      # Hmotnost míčku
x = 50       # Aktuální poloha (výchylka)
v = 0        # Rychlost

while True:
    # 1. Vypočítáš aktuální sílu podle polohy
    F = -k * x 
    
    # 2. Zjistíš zrychlení (a = F / m)
    a = F / m
    
    # 3. Aktualizuješ rychlost a polohu
    v = v + a
    x = x + v
    
    vykresli(x)
