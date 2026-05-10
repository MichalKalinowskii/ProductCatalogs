# ProductCatalogs

Backend (stworzone przy użyciu Visual Studio 2022):
- .NET 8
Frontend (stworzone przy użyciu Visual Studio Code):
- Angular 20
- Node.js v24.5.0
(UWAGA aby uruchmić aplikacje front-ową należy wpierw pobrać pakiety za pomocą komendy "npm install")


Architektura aplikacji zostały wykonona z wykorzystaniem architektury modularnego monolitu.
Zamiast rzucania wyjątkami w logice biznesowej, aplikacja zwraca czytelne obiekty stanu (Result),
    w celu uproszczenia zadania te błędy biznesowe zwracane są do konsoli przeglądarki.