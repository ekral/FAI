# Základní pojmy MVVM

- **View** představuje to co vidí uživatel na obrazovce. Obsahuje elementy uživatelského rozhraní jako například ```TextBlock``` nebo ```Button```.
- **ViewModel** obsahuje property, Commandy (zatím jsme neprobrali) a metody na které binduje View. Pokud se ve ViewModelu změní hodnota nějaké property, tak o této změně ViewModel informuje informuje View pomocí eventu ```NotifyPropertyChanged```. ViewModel představuje prostředníka mezi View a Modelem, připravuje data pro zobrazení a reaguje na akce uživatele, ale nepoužívá elementy uživatelského rozhraní.
- **Model** představuje aplikační logiku aplikace. Například složitý výpočet parametrů životního pojištění. Model pracuje z hlediska logiky aplikace a vůbec s nezajímá o uživatelské rozhraní.
- Event **NotifyPropertyChanged** používá ViewModel k tomu aby infomoval (notifikoval) View o změnách ve svých propertách. Je součástí rozhraní ```INotifyPropertyChanged```.

Použití MVVM je ve frameworku Avalonia, ale i dalších dobrovolné a nemusíme jej používat. Cílem je mít modulární návrh aplikace, tak abychom mohli jednoduše měnit ViewModel například za testovací verzi a také vyvíjet uživatelské rozhran nezávisle na vývoji ViewModelu.
