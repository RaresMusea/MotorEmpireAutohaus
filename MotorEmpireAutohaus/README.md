# MotorEmpireAutohaus
O aplicație cros-platform, dezvoltată folosind .NET MAUI și C# ce figurează o soluție e-commerce, o aplicație de vânzări auto second hand la nivel global.

## About
Aplicația are o arhitectură relativ simplă. Transferul datelor în cazul acesteia se va realiza doar prin operații de CRUD (Create, Read, Update, Delete) asupra unei baze de date MySQL, ce va fi în permanență conectată la aplicația cross-platform.

## Pe ce ecosisteme poate rula aplicația?
Aplicația este cross-platform, ceea ce înseamnă că aceasta profită la maxim de facilitățile ecosistemului .NET Core, aceasta putând fi deployată atât pe Windows, MAC OS, dar și pe sisteme de operare Mobile (implicit, iOS și Android).

## Tehnologii utilizate
### Limbaje
-- C# 9.0 pe platforma .NET 6.0 (LTS)

### Storage
-- MySQL 8.0

### NuGet Packages și dependințe
-- MySQL Connector for C#, pentru comunicarea dintre aplicația cross-platform și storage
-- Microsoft MAUI Extensions, preinstalat
-- Micrososft MAUI Dependencies, preinstalat
-- Community Toolkit MVVM (Model-View View-Model), pentru a facilita binding-ul entităților pe markup
-- Microsoft Windows SDK Build Tools

## Posibilitati de dezvoltare
-- Implementarea unui serviciu RESTful care să administreze mai eficient arhitectura MVVM a aplicației.