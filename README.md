# MotorEmpireAutohaus
O aplicație cross - platform, dezvoltată folosind .NET MAUI și C# ce figurează o soluție e-commerce, o aplicație de vânzări auto second hand la nivel global.

## About
 Aplicația are la baza pattern-ul arhitectural MVVM (Model View ViewModel).
 Fiecare componenta (pagina) din cadrul aplicatiei reprezinta un View. Acesta descrie felul in care aplicatia va arata in functie de ecosistemul pe care ruleaza. Pentru design si styling s-a folosit in principal XAML, iar in anumite sectiuni in care markup-ul nu a permis randarea conditionata a layout-ului dependent de un anume OS, acesta a fost completat de cod scris in fisierul C# aferent view-ului (ContentPage-ului). Toate View-urile pot fi regasite in fisierul intitulat View.
 Pentru a putea realiza binding-ul intre elementele grafice din View si structurile ce vor stoca informatii despre acestea, cat si pentru a efectua un state management eficient, pentru fiecare View s-a implementat si cate un ViewModel. ViewModel-ul nu reprezinta nimic altceva decat o clasa C# in care este scrisa logica de business care permite transferul datelor de la interfata catre baza de date si reciproc. Mai mult decat atat, fiecare view model este responsabil de state management-ul fiecarui view caruia i se adreseaza. In acest fel, backend-ul aplicatiei este constant notificat de modificarile ce apar in frontend (pe interfata grafica), acolo unde este cazul. S-a obtinut acest lucru prin utilizarea de Observables, dar si a unor interfete definite in core-ul library-ului MAUI, precum INotifyPropertyChanged. Pentru a eficientiza workload-ul, am utilizat MVVM Community Toolkit, care permite declararea unor clase partiale si referirea acestor notificari/metode/Observables prin intermediul unor anotari. Pe baza acestora, se va autogenera intr-o maniera cat se poate de eficienta clasa finala care va contine implementari ale tuturor acestor pattern-uri anterior mentionate. Fiecare View Model poate fi regasit in fisierul View Model din root-ul proiectului.
 Pentru persistenta datelor, fiecare entitate majora din cadrul aplicatiei (Cont de utilizator/Vehicul/Anunt) implementeaza cate un Model. Un model nu este nimic altceva decat o clasa intermediara, un wrapper ce ne va permite o comunicare mai facila intre view model si baza de date. Facand o trimitere catre pattern-ul arhitectural MVC, folosit in special in cadrul aplicatiilor Web, un Model MVVM poate fi asociat cu modelul MVC, el nefiind altceva decat un element de mapare si transfer al datelor (DTO-Data Transfer Object). Fiecare astfel de model poate fi regasit in fisierul Models din root-ul proiectului.
 Prelucrarea datelor stocate in modele reprezinta de departe cel mai important aspect din cadrul aplicatiei, intrucat majoritatea datelor din cadrul aplicatiei sunt inserate/obtinute dintr-o baza de date locala. Pentru acest lucru, s-a lucrat exclusiv pe baza de date, prin intermediul driver-ului MySQL Connector N, fara a utiliza niciun ORM. Operatiile la nivel de storage din fiecare entitate a aplicatiei au fost integrate in cate o clasa-serviciu. Aceste asa-numite servicii vor putea fi regasite in fisierul Services. De acolo, ele vor fi adaugate in View Model, prin intermediul DI (Dependency Injection/Injectie de dependinte).
 Pentru datele de dimensiuni mari (fotografii de profil ale user-ilor, fotografii ale masinilor), baza de date locala nu realizeaza o stocare efectiva a acestora, ci o stocare a unui URL catre adresa lor dintr-un bucket de tip Firebase Cloud Storage.
Transferul datelor în cadrul aplicatiei se va realiza doar prin operații de CRUD (Create, Read, Update, Delete) asupra unei baze de date MySQL, ce va fi în permanență conectată la aplicația cross-platform.

## Pe ce ecosisteme poate rula aplicația?
Aplicația este cross-platform, ceea ce înseamnă că aceasta profită la maxim de facilitățile ecosistemului .NET Core, aceasta putând fi deployată atât pe Windows, MAC OS, dar și pe sisteme de operare Mobile (implicit, iOS și Android).

## Tehnologii utilizate
### Limbaje
-- C# 9.0 pe platforma .NET 7.0

### Storage
-- MySQL 8.0
-- Firebase & Firebase Cloud Storage

### NuGet Packages și dependințe
-- MySQL Connector N for C#, pentru comunicarea dintre aplicația cross-platform și storage
-- Microsoft MAUI Extensions, preinstalat
-- Micrososft MAUI Dependencies, preinstalat
-- Community Toolkit MVVM (Model-View View-Model), pentru a facilita binding-ul entităților pe markup
-- Microsoft Windows SDK Build Tools
-- Firebase Package

## Posibilitati de dezvoltare
-- Implementarea unui serviciu RESTful care să administreze mai eficient arhitectura MVVM a aplicației.
