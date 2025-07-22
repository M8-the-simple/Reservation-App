Treba se unutar .NET cli-a navigirati do .\Reservation\ te pokrenuti aplikaciju.

Stvaranje konktrenih rezervacija se dešava unutar .http datoteka. U tim datotekama se nalaze već spremni JWT tokeni unutar kojih se nalaze potrebne uloge za moći pristupiti 'endpointovima', te 'claim', 'id'.
Za dohvaćanje rezeravacija, ali i za njihovo stvaranje se koristi kombinacija URL-a i 'id' 'claim' u JWT tokenu.
JWT tokeni unutar .http dokumenta se poredani silazno tj. prvi ima 'id' 1, a zadnji 3.

Tablica pacijenata:
1	Julia	Jules
2	Jack	Jones
3	Miles	Morales

Tablica doktora:
1	Mike	Meyers
2	Charles	Manson
3	Maxim	House
