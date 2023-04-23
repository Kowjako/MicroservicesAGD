# 💍 Aplikacja backendowa sklepu AGD
Aplikacja backednowa sklepu AGD zaimplementowana w architekturze mikroserwisowej zgodnie z Clean Architecture, DDD, CQRS. Mikroserwisy zaimplementowane w ASP.NET Core Web API, użyte różne SQL/NoSQL rozwiązania dla poszczególnych serwisów. Został zaimplementowany API Gateway oraz dodany broker wiadomości RabbitMQ do asynchronicznej komunikacji.

## ☂️ Architektura aplikacji
![microservices-arch](https://user-images.githubusercontent.com/19534189/233836196-fea3257f-63ed-4f89-8195-664bf48adab8.png)

## ☂️ Clean Architecture, DDD, CQRS
Microserwis Ordering.API został zaimplementowany zgodnie z DDD, CQRS, Clean Architecture. Wyodrębniona wartswa domenowa Ordering.Domain, przedstawiająca klasę bazową dla obiektów wartości (Value Object)
oraz encje bazowe. Projekt Ordering.Application przedstawia wartswę Use Cases, czyli tutaj zdefinowane polecenia oraz komendy zgodzine z CQRS (za pomocą MediatR) oraz zdefiniowane kontrakty
do warstwy infrastruktury oraz dostępu do danych, które będą potrzebne dla funkcjonowania. Projekt Ordering.Insfrastructure implementuje kontraktry projektu Ordering.Application dotyczące infrastruktury zarówno
jak i dostępu do danych. Wartswa prezentacji (Ordering.API) przedstawia wystawione REST API dla użytkowników oraz odpowiada za rejestrację wszystkich zależności w kontenerze Dependency Injection.  
<img src="https://user-images.githubusercontent.com/19534189/233836464-eab4d473-b80e-4367-80a7-55a987f38574.png"  width="400" height="400">  
## ☂️ Użyte technologie
1. **ASP.NET Core 6 Web API** - trzy serwisy (Basket.API, Ordering.API, Catalog.API) oraz Discount.API - do celów testowych (w rzeczywistości używany projekt to Discount.Grpc,) zaimplementowane za pomocą
Web API i wystawiają REST API, pomimo tego każdy serwis ma swój Dockerfile dzięki czemu możemy zbuildować obraz takiego serwisu żeby następnie go umieścić w infrastrukturze Dockerowej (tam launchSettings.json już nie odgrywa żadnej roli więc definiujemy swoje porty w docker-compose.override.yml).
2. **ASP.NET Grpc** - serwis zniżek Discount.Grpc jest zaimplementowany jako Grpc serwis (a nie REST API), dzięki temu, nasz Basket.API podczas dodania produktów do koszyka, uderza do Grpc serwisu
żeby sprawdzić czy dany artykuł ma zniżkę, gdy tak to zastosuje ją.
3. **Redis, PostgreSQL, SQL Server, MongoDB**
4. **Dapper** - użyty w Discount.API i Discount.Grpc jako Micro-ORM dla PostgreSQL.
5. **Ocelot Api Gateway** - zamiast Nginx, występuje w roli reverse-proxy wystawia swoje API które przekierowuje zapytania do odpowiednich serwisów.
6. **RabbitMQ** - broker do asynchronicznej komunikacji między serwisem Basket.API oraz Ordering.API, po zatwierdzeniu koszyka publikujemy zdarzenie do kolejki RabbitMQ, z kolei Ordering.API
występuje subskrybentem tego zdarzenia, czyli po zatwierdzeniu koszyka, serwis zamówień odbierze dane zdarzenie oraz utworzy zamówienie na bazie SQL Server.
7. **Docker** - cała infrastruktura aplikacji
8. **AutoMapper, MediatR, FluentValidaton, Swagger**

## ☂️ Jak uruchomić aplikację
1. Mieć zainstalowany Docker Desktop
2. Uruchomic kontenery:  
**docker-compose -f docker.compose.yml -f docker-compose.override.yml up -d**  
Następnie nasze serwisy w środowisku docker'a będą dostępne pod kolejnymi portami:
* **Catalog API (lokalny debugger na :5000) -> http://localhost:8000/swagger/index.html**
* **Basket API (lokalny debugger na :5001) -> http://localhost:8001/swagger/index.html**
* **Discount API (lokalny debugger na :5002) -> http://localhost:8002/swagger/index.html**
* **Ordering API (lokalny debugger na :5004) -> http://localhost:8004/swagger/index.html**
* **Shopping.Aggregator (lokalny debugger na :5005) -> http://localhost:8005/swagger/index.html**
* **API Gateway (lokalny debugger na :5010) -> http://localhost:8010/Catalog**
* **Rabbit Management Dashboard -> http://localhost:15672**   -- guest/guest
* **PgAdmin PostgreSQL -> http://localhost:5050**   -- admin@test.com/admin1234  


W przypadku potrzeby połączyć się z SQL Server (od Ordering.API) poprzez SSMS, server: **localhost,1434** login: **sa** hasło: **Jko3va-D9821jhsvGD**
