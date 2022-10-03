# Základní pojmy MVVM

- **View** View představuje to co vidí uživatel na obrazovce. Obsahuje elementy uživatelského rozhraní jako například ```TextBlock``` nebo ```Button```.
- ViewModel - ViewModel obsahuje property, Commandy (zatím jsme neprobrali) a metody na které binduje View. Pokud se ve ViewModelu změní hodnota nějaké property, tak o této změně ViewModel informuje informuje View pomocí event NotifyPropertyChanged. ViewModel představuje prostředníka mezi View a Modelem, připravuje data pro zobrazení a reaguje na akce uživatele, ale nepoužívá elementy uživatelského rozhraní.
- Model - Model představuje aplikační logiku aplikace. Například složitý výpočet parametrů životního pojištění. Model pracuje z hledsika logiky aplikace a vůbec s nezajímá o uživatelské rozhraní.
- NotifyPropertyChanged Event
